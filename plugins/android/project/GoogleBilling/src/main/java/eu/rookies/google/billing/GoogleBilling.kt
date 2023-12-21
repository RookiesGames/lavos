package eu.rookies.google.billing

import android.util.Log
import androidx.annotation.NonNull
import com.android.billingclient.api.AcknowledgePurchaseParams
import com.android.billingclient.api.AcknowledgePurchaseResponseListener
import com.android.billingclient.api.BillingClient
import com.android.billingclient.api.BillingClient.BillingResponseCode
import com.android.billingclient.api.BillingClient.ProductType
import com.android.billingclient.api.BillingClientStateListener
import com.android.billingclient.api.BillingFlowParams
import com.android.billingclient.api.BillingResult
import com.android.billingclient.api.ConsumeParams
import com.android.billingclient.api.ConsumeResponseListener
import com.android.billingclient.api.ProductDetails
import com.android.billingclient.api.Purchase
import com.android.billingclient.api.Purchase.PurchaseState
import com.android.billingclient.api.PurchasesResponseListener
import com.android.billingclient.api.PurchasesUpdatedListener
import com.android.billingclient.api.QueryProductDetailsParams
import com.android.billingclient.api.QueryPurchasesParams
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class GoogleBilling(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = GoogleBilling::class.java.simpleName

    private lateinit var billingClient: BillingClient

    private var queryProductsStatus = QueryProductsStatus.None
    private lateinit var productDetailsList: List<ProductDetails>

    private var querySubscriptionsStatus = QuerySubscriptionsStatus.None
    private lateinit var subscriptionDetailsList: List<ProductDetails>

    private var purchaseProgressStatus = PurchaseProgressStatus.None
    private var queryPurchaseStatus = QueryPurchaseStatus.None
    private var consumePurchaseStatus = ConsumeStatus.None
    private var acknowledgePurchaseStatus = AcknowledgePurchaseStatus.None

    private lateinit var pendingPurchases: MutableList<Purchase>
    private lateinit var pendingConsumables: List<String>

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        val purchasesUpdatedListener =
            PurchasesUpdatedListener { billingResult, purchases ->
                if (billingResult.responseCode != BillingResponseCode.OK) {
                    Log.d(pluginName, "Purchase failed. Error: ${billingResult.responseCode}")
                    purchaseProgressStatus = PurchaseProgressStatus.Error
                } else if (purchases == null) {
                    Log.d(pluginName, "No purchase detected")
                    purchaseProgressStatus = PurchaseProgressStatus.None
                } else {
                    // Do nothing?
                    purchaseProgressStatus = PurchaseProgressStatus.Completed
                }
            }
        //
        billingClient = BillingClient.newBuilder(activity!!.applicationContext)
            .setListener(purchasesUpdatedListener)
            .enablePendingPurchases()
            .build()
        //
        connect()
    }

    override fun onMainDestroy() {
        super.onMainDestroy()
        billingClient.endConnection()
    }

    private fun isConnected(): Boolean = billingClient.isReady

    private fun connect() {
        val billingClientStateListener = object : BillingClientStateListener {
            override fun onBillingSetupFinished(billingResult: BillingResult) {
                if (billingResult.responseCode == BillingResponseCode.OK) {
                    Log.d(pluginName, "Billing service connected")
                } else {
                    Log.d(
                        pluginName,
                        "Billing service connection failed. Error: ${billingResult.responseCode}"
                    )
                }
            }

            override fun onBillingServiceDisconnected() {
                Log.d(pluginName, "Billing service disconnected")
                billingClient.startConnection(this)
            }
        }
        //
        billingClient.startConnection(billingClientStateListener)
    }

    //////////////////
    // Product query
    //////////////////

    private fun queryDetails(
        type: String,
        productIds: Array<String>,
        action: (BillingResult, List<ProductDetails>) -> Unit
    ) {
        var productList = ArrayList<QueryProductDetailsParams.Product>()
        productIds.forEach {
            productList.add(
                QueryProductDetailsParams.Product.newBuilder()
                    .setProductId(it)
                    .setProductType(type)
                    .build()
            )
        }
        //
        val params = QueryProductDetailsParams.newBuilder()
            .setProductList(productList)
            .build()
        //
        billingClient.queryProductDetailsAsync(params) { billingResult, detailsList ->
            Log.d(pluginName, "Query of $type finished with result: $billingResult")
            action(billingResult, detailsList)
        }
    }

    @UsedByGodot
    fun getQueryProductsStatus(): Int = queryProductsStatus.id

    @UsedByGodot
    fun queryProductsDetails(productIds: Array<String>) {
        queryProductsStatus = QueryProductsStatus.InProgress
        productDetailsList = emptyList()
        //
        queryDetails(ProductType.INAPP, productIds) { billingResult, productList ->
            if (billingResult.responseCode == BillingResponseCode.OK) {
                productDetailsList = productList
                queryProductsStatus = QueryProductsStatus.Completed
            } else {
                queryProductsStatus = QueryProductsStatus.Error
            }
        }
    }

    @UsedByGodot
    fun getProducts(): Array<String> {
        val array = productDetailsList.map { ProductHelper.toJson(it).toString() }
        return array.toTypedArray()
    }

    private fun getProductInternal(id: String): ProductDetails? =
        productDetailsList.find { it.productId == id }

    @UsedByGodot
    fun getProduct(id: String): String {
        val product = getProductInternal(id)
        return if (product != null) {
            ProductHelper.toJson(product).toString()
        } else {
            ""
        }
    }

    @UsedByGodot
    fun getQuerySubscriptionsStatus(): Int = querySubscriptionsStatus.id

    @UsedByGodot
    fun querySubscriptionsDetails(productIds: Array<String>) {
        querySubscriptionsStatus = QuerySubscriptionsStatus.InProgress
        subscriptionDetailsList = emptyList()
        //
        queryDetails(ProductType.SUBS, productIds) { billingResult, subscriptionList ->
            if (billingResult.responseCode == BillingResponseCode.OK) {
                subscriptionDetailsList = subscriptionList
                querySubscriptionsStatus = QuerySubscriptionsStatus.Completed
            } else {
                querySubscriptionsStatus = QuerySubscriptionsStatus.Error
            }
        }
    }

    @UsedByGodot
    fun getSubscriptions(): Array<String> {
        val array = subscriptionDetailsList.map { ProductHelper.toJson(it).toString() }
        return array.toTypedArray()
    }

    private fun getSubscriptionInternal(id: String): ProductDetails? =
        subscriptionDetailsList.find { it.productId == id }

    @UsedByGodot
    fun getSubscription(id: String): String {
        val product = getSubscriptionInternal(id)
        return if (product != null) {
            ProductHelper.toJson(product).toString()
        } else {
            ""
        }
    }

    ////////////////////
    // Purchasing flow

    @UsedByGodot
    fun purchaseInProgress(): Boolean = purchaseProgressStatus == PurchaseProgressStatus.InProgress

    @UsedByGodot
    fun getPurchaseStatus(): Int = purchaseProgressStatus.id

    @UsedByGodot
    fun purchaseProduct(id: String): Boolean {
        if (purchaseInProgress()) {
            Log.d(pluginName, "Purchase in progress")
            return false
        }
        val product = getProductInternal(id)
        if (product == null) {
            Log.e(pluginName, "Product $id not found")
            return false
        }
        //
        val productDetailsParamsList = listOf(
            BillingFlowParams.ProductDetailsParams.newBuilder()
                .setProductDetails(product)
                .build()
        )
        //
        val billingFlowParam = BillingFlowParams.newBuilder()
            .setProductDetailsParamsList(productDetailsParamsList)
            .build()
        //
        val billingResult =
            billingClient.launchBillingFlow(activity!!, billingFlowParam)
        Log.d(pluginName, "Purchase flow launched with result: ${billingResult.responseCode}")
        //
        return billingResult.responseCode == BillingResponseCode.OK
    }

    //////////////
    // Purchases

    @UsedByGodot
    fun getPendingPurchases(): Array<String> =
        pendingPurchases.map { purchase -> purchase.purchaseToken }.toTypedArray()

    @UsedByGodot
    fun getQueryPurchasesStatus(): Int = queryPurchaseStatus.id

    private fun queryPurchases(
        type: String,
        action: (BillingResult, List<Purchase>) -> Unit
    ) {
        val queryPurchasesParams = QueryPurchasesParams.newBuilder()
            .setProductType(type)
            .build()
        //
        val purchasesResponseListener = PurchasesResponseListener { billingResult, purchases ->
            Log.d(
                pluginName,
                "Query purchases completed with result: ${billingResult.responseCode}"
            )
            action(billingResult, purchases)
        }
        //
        billingClient.queryPurchasesAsync(queryPurchasesParams, purchasesResponseListener)
    }

    @UsedByGodot
    fun queryPurchases() {
        pendingPurchases = mutableListOf()
        var step = 0
        fun completeQuery() {
            step++
            queryPurchaseStatus = if (step == 2) {
                QueryPurchaseStatus.Completed
            } else {
                queryPurchaseStatus
            }
        }
        //
        queryPurchaseStatus = QueryPurchaseStatus.InProgress
        //
        queryPurchases(ProductType.INAPP) { iapBillingResult, iapPurchases ->
            if (iapBillingResult.responseCode == BillingResponseCode.OK) {
                pendingPurchases.addAll(iapPurchases)
                completeQuery()
            } else {
                queryPurchaseStatus = QueryPurchaseStatus.Error
            }
        }
        queryPurchases(ProductType.SUBS) { subsBillingResult, subsPurchases ->
            if (subsBillingResult.responseCode == BillingResponseCode.OK) {
                pendingPurchases.addAll(subsPurchases)
                completeQuery()
            } else {
                queryProductsStatus = QueryProductsStatus.Error
            }
        }
    }

    private fun getPurchase(token: String): Purchase? =
        pendingPurchases.find { purchase -> purchase.purchaseToken == token }

    ////////////////
    // Consumables

    @UsedByGodot
    fun getConsumePurchaseStatus(): Int = consumePurchaseStatus.id

    @UsedByGodot
    fun getPendingConsumables(): Array<String> {
        val array = pendingConsumables.toTypedArray()
        pendingConsumables = emptyList()
        consumePurchaseStatus = ConsumeStatus.None
        return array
    }

    @UsedByGodot
    fun consumePurchase(token: String): Int {
        var purchase = getPurchase(token)
        if (purchase == null) {
            Log.d(pluginName, "No purchase with token $token could be found")
            consumePurchaseStatus = ConsumeStatus.Error
            return consumePurchaseStatus.id
        }
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.d(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            consumePurchaseStatus = ConsumeStatus.Error
            return consumePurchaseStatus.id
        }
        //
        val consumeParams = ConsumeParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        //
        val consumeResponseListener = ConsumeResponseListener { billingResult, consumedToken ->
            Log.d(
                pluginName,
                "Consume purchase completed with result: ${billingResult.responseCode}"
            )
            if (billingResult.responseCode == BillingResponseCode.OK) {
                purchase = getPurchase(consumedToken)
                if (purchase != null) {
                    // Remove purchase from pending list
                    pendingPurchases.filter { purchase -> purchase.purchaseToken != token }
                    pendingConsumables = purchase!!.products
                }
                consumePurchaseStatus = ConsumeStatus.Completed
            }
        }
        //
        billingClient.consumeAsync(consumeParams, consumeResponseListener)
        consumePurchaseStatus = ConsumeStatus.InProgress
        return consumePurchaseStatus.id
    }

    @UsedByGodot
    fun getAcknowledgePurchaseStatus(): Int = acknowledgePurchaseStatus.id

    @UsedByGodot
    fun acknowledgePurchase(token: String) {
        acknowledgePurchaseStatus = AcknowledgePurchaseStatus.None
        //
        var purchase = getPurchase(token)
        if (purchase == null) {
            Log.d(pluginName, "No purchase with token $token could be found")
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Error
            return
        }
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.d(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Error
            return
        }
        if (purchase.isAcknowledged) {
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Completed
            return
        }
        //
        val acknowledgePurchaseParams = AcknowledgePurchaseParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        val acknowledgePurchaseResponseListener = AcknowledgePurchaseResponseListener {
            if (it.responseCode == BillingResponseCode.OK) {
                acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Completed
            } else {
                acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Error
            }
        }
        //
        acknowledgePurchaseStatus = AcknowledgePurchaseStatus.InProgress
        billingClient.acknowledgePurchase(
            acknowledgePurchaseParams,
            acknowledgePurchaseResponseListener
        )
    }
}