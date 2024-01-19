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

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        val purchasesUpdatedListener =
            PurchasesUpdatedListener { billingResult, purchases ->
                purchaseProgressStatus = if (billingResult.responseCode != BillingResponseCode.OK) {
                    Log.e(pluginName, "Purchase failed. ${billingResult.debugMessage}")
                    PurchaseProgressStatus.Error
                } else if (purchases == null) {
                    Log.e(pluginName, "No purchase detected")
                    PurchaseProgressStatus.None
                } else {
                    PurchaseProgressStatus.Completed
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
                if (billingResult.responseCode != BillingResponseCode.OK) {
                    val msg =
                        "Billing service connection failed. ${billingResult.debugMessage}"
                    Log.e(pluginName, msg)
                }
            }

            override fun onBillingServiceDisconnected() {
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
                Log.e(
                    pluginName,
                    "Failed to query product details. ${billingResult.debugMessage}"
                )
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
                Log.e(
                    pluginName,
                    "Failed to query subs details. ${billingResult.debugMessage}"
                )
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
    fun getPurchaseStatus(): Int = purchaseProgressStatus.id

    @UsedByGodot
    fun purchaseProduct(id: String): Boolean {
        if (purchaseProgressStatus == PurchaseProgressStatus.InProgress) {
            Log.d(pluginName, "Purchase in progress")
            return false
        }
        val product =
            getProductInternal(id)
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
        //
        return billingResult.responseCode == BillingResponseCode.OK
    }

    @UsedByGodot
    fun purchaseSubscription(id: String, offerToken: String): Boolean {
        if (purchaseProgressStatus == PurchaseProgressStatus.InProgress) {
            Log.d(pluginName, "Purchase in progress")
            return false
        }
        val product = getSubscriptionInternal(id)
        if (product == null) {
            Log.e(pluginName, "Subscription $id not found")
            return false
        }
        //
        val productDetailsParamsList = listOf(
            BillingFlowParams.ProductDetailsParams.newBuilder()
                .setProductDetails(product)
                .setOfferToken(offerToken)
                .build()
        )
        //
        val billingFlowParam = BillingFlowParams.newBuilder()
            .setProductDetailsParamsList(productDetailsParamsList)
            .build()
        //
        val billingResult =
            billingClient.launchBillingFlow(activity!!, billingFlowParam)
        //
        return billingResult.responseCode == BillingResponseCode.OK
    }

    //////////////
    // Pending Purchases

    @UsedByGodot
    fun getPendingPurchases(): Array<String> =
        pendingPurchases.map { purchase -> purchase.originalJson }.toTypedArray()

    @UsedByGodot
    fun getQueryPurchasesStatus(): Int = queryPurchaseStatus.id

    private fun doQueryPurchases(
        type: String,
        action: (BillingResult, List<Purchase>) -> Unit
    ) {
        val queryPurchasesParams = QueryPurchasesParams.newBuilder()
            .setProductType(type)
            .build()
        //
        val purchasesResponseListener = PurchasesResponseListener { billingResult, purchases ->
            action(billingResult, purchases)
        }
        //
        billingClient.queryPurchasesAsync(queryPurchasesParams, purchasesResponseListener)
    }

    @UsedByGodot
    fun queryPurchases() {
        pendingPurchases = mutableListOf()
        queryPurchaseStatus = QueryPurchaseStatus.InProgress
        //
        doQueryPurchases(ProductType.INAPP) { iapBillingResult, iapPurchases ->
            if (iapBillingResult.responseCode == BillingResponseCode.OK) {
                pendingPurchases.addAll(iapPurchases)
            } else {
                Log.e(pluginName, "Failed to query INAPPs. ${iapBillingResult.debugMessage}")
                queryPurchaseStatus = QueryPurchaseStatus.Error
            }
            //
            doQueryPurchases(ProductType.SUBS) { subsBillingResult, subsPurchases ->
                if (subsBillingResult.responseCode == BillingResponseCode.OK) {
                    pendingPurchases.addAll(subsPurchases)
                    queryPurchaseStatus = QueryPurchaseStatus.Completed
                } else {
                    Log.e(
                        pluginName,
                        "Failed to query SUBs. Error ${subsBillingResult.debugMessage}"
                    )
                    queryProductsStatus = QueryProductsStatus.Error
                }
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
    fun consumePurchase(token: String): Int {
        var purchase = getPurchase(token)
        if (purchase == null) {
            Log.e(pluginName, "No purchase with token $token could be found")
            consumePurchaseStatus = ConsumeStatus.Error
            return consumePurchaseStatus.id
        }
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.e(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            consumePurchaseStatus = ConsumeStatus.Error
            return consumePurchaseStatus.id
        }
        //
        val consumeParams = ConsumeParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        //
        val consumeResponseListener = ConsumeResponseListener { billingResult, consumedToken ->
            consumePurchaseStatus = if (billingResult.responseCode == BillingResponseCode.OK) {
                ConsumeStatus.Completed
            } else {
                Log.e(pluginName, "Failed to consume purchase. ${billingResult.debugMessage}")
                ConsumeStatus.Error
            }
        }
        //
        consumePurchaseStatus = ConsumeStatus.InProgress
        billingClient.consumeAsync(consumeParams, consumeResponseListener)
        return consumePurchaseStatus.id
    }

    @UsedByGodot
    fun getAcknowledgePurchaseStatus(): Int = acknowledgePurchaseStatus.id

    @UsedByGodot
    fun acknowledgePurchase(token: String) {
        acknowledgePurchaseStatus = AcknowledgePurchaseStatus.None
        //
        val purchase = getPurchase(token)
        if (purchase == null) {
            Log.e(pluginName, "No purchase found to acknowledge. $token")
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Error
            return
        }
        //
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.e(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Error
            return
        }
        //
        if (purchase.isAcknowledged) {
            acknowledgePurchaseStatus = AcknowledgePurchaseStatus.Completed
            return
        }
        //
        val acknowledgePurchaseParams = AcknowledgePurchaseParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        val acknowledgePurchaseResponseListener =
            AcknowledgePurchaseResponseListener { billingResult ->
                acknowledgePurchaseStatus =
                    if (billingResult.responseCode == BillingResponseCode.OK) {
                        AcknowledgePurchaseStatus.Completed
                    } else {
                        Log.e(
                            pluginName,
                            "Failed to acknowledged purchase. ${billingResult.debugMessage}"
                        )
                        AcknowledgePurchaseStatus.Error
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