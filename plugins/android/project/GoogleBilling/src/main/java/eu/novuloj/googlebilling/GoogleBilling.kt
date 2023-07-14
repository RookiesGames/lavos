package eu.novuloj.googlebilling

import android.util.Log
import androidx.annotation.NonNull
import com.android.billingclient.api.AcknowledgePurchaseParams
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
import com.android.billingclient.api.acknowledgePurchase
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class GoogleBilling(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = "GoogleBilling"

    private lateinit var billingClient: BillingClient

    private lateinit var productDetailsList: List<ProductDetails>
    private lateinit var subscriptionDetailsList: List<ProductDetails>

    private lateinit var pendingPurchases: List<Purchase>
    private lateinit var pendingConsumables: List<String>

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        val purchasesUpdatedListener =
            PurchasesUpdatedListener { billingResult, purchases ->
                if (billingResult.responseCode != BillingResponseCode.OK) {
                    Log.d(pluginName, "Purchase failed. Error: ${billingResult.responseCode}")
                } else if (purchases == null) {
                    Log.d(pluginName, "No purchase detected")
                } else {
                    // Do nothing?
                }
            }
        //
        billingClient = BillingClient.newBuilder(godot.requireContext())
            .setListener(purchasesUpdatedListener)
            .enablePendingPurchases()
            .build()
    }

    @UsedByGodot
    fun isConnected(): Boolean = billingClient.isReady

    @UsedByGodot
    fun connect() {
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
            }
        }
        //
        billingClient.startConnection(billingClientStateListener)
    }

    @UsedByGodot
    fun disconnect() = billingClient.endConnection()

    // Product query

    private fun query(
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
    fun hasProducts(): Boolean = productDetailsList.isNotEmpty()

    @UsedByGodot
    fun queryProducts(productIds: Array<String>) {
        productDetailsList = emptyList()
        //
        query(ProductType.INAPP, productIds) { billingResult, productList ->
            Log.d(pluginName, "Query products details finished with result: $billingResult")
            productDetailsList = productList
        }
    }

    @UsedByGodot
    fun hasSubscriptions(): Boolean = subscriptionDetailsList.isNotEmpty()

    @UsedByGodot
    fun querySubscriptions(productIds: Array<String>) {
        subscriptionDetailsList = emptyList()
        //
        query(ProductType.SUBS, productIds) { billingResult, subscriptionList ->
            Log.d(pluginName, "Query subscription details finished with result: $billingResult")
            subscriptionDetailsList = subscriptionList
        }
    }

    // Product Details

    private fun getProduct(id: String): ProductDetails =
        productDetailsList.find { it.productId == id }!!

    @UsedByGodot
    fun getProductDescription(id: String): String = getProduct(id).description

    @UsedByGodot
    fun getProductName(id: String): String = getProduct(id).name

    @UsedByGodot
    fun getProductType(id: String): String = getProduct(id).productType

    @UsedByGodot
    fun getProductTitle(id: String): String = getProduct(id).title

    // One Time Offer Details

    private fun getOneTimeOfferDetails(id: String): ProductDetails.OneTimePurchaseOfferDetails =
        getProduct(id).oneTimePurchaseOfferDetails!!

    @UsedByGodot
    fun getProductFormattedPrice(id: String): String = getOneTimeOfferDetails(id).formattedPrice

    @UsedByGodot
    fun getProductPriceCurrencyCode(id: String): String =
        getOneTimeOfferDetails(id).priceCurrencyCode

    @UsedByGodot
    fun getProductPriceAmount(id: String): Float =
        getOneTimeOfferDetails(id).priceAmountMicros.toFloat()

    // Subscription Offer Details

    private fun getSubscriptionOfferDetails(id: String): List<ProductDetails.SubscriptionOfferDetails> = getProduct(id).subscriptionOfferDetails!!

    // Purchasing flow

    @UsedByGodot
    fun purchaseProduct(id: String): Boolean {
        val activity = godot.activity
        val product = getProduct(id)
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
            billingClient.launchBillingFlow(godot.requireActivity(), billingFlowParam)
        Log.d(pluginName, "Purchase flow launched with result: ${billingResult.responseCode}")
        //
        return billingResult.responseCode == BillingResponseCode.OK
    }

    // Purchases

    @UsedByGodot
    fun hasPendingPurchases(): Boolean = pendingPurchases.isNotEmpty()

    @UsedByGodot
    fun getPendingPurchases(): Array<String> =
        pendingPurchases.map { purchase -> purchase.purchaseToken }.toTypedArray()

    @UsedByGodot
    fun queryPurchases() {
        pendingPurchases = emptyList()
        //
        var queryPurchasesParams = QueryPurchasesParams.newBuilder()
            .setProductType(ProductType.INAPP)
            .build()
        //
        val purchasesResponseListener = PurchasesResponseListener { billingResult, purchases ->
            Log.d(
                pluginName,
                "Query purchases completed with result: ${billingResult.responseCode}"
            )
            if (billingResult.responseCode == BillingResponseCode.OK) {
                pendingPurchases = purchases
            }
        }
        //
        billingClient.queryPurchasesAsync(queryPurchasesParams, purchasesResponseListener)
    }

    private fun getPurchase(token: String): Purchase? =
        pendingPurchases.find { purchase -> purchase.purchaseToken == token }

    // Consumables

    @UsedByGodot
    fun hasPendingConsumables(): Boolean = pendingConsumables.isNotEmpty()

    @UsedByGodot
    fun getPendingConsumables(): Array<String> {
        val array = pendingConsumables.toTypedArray()
        pendingConsumables = emptyList()
        return array
    }

    @UsedByGodot
    fun consumePurchase(token: String): Boolean {
        var purchase = getPurchase(token)
        if (purchase == null) {
            Log.d(pluginName, "No purchase with token $token could be found")
            return false
        }
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.d(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            return false
        }
        //
        val consumeParams = ConsumeParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        //
        val consumeResponseListener = ConsumeResponseListener { billingResult, token ->
            Log.d(
                pluginName,
                "Consume purchase completed with result: ${billingResult.responseCode}"
            )
            if (billingResult.responseCode == BillingResponseCode.OK) {
                purchase = getPurchase(token)
                if (purchase != null) {
                    pendingConsumables = purchase!!.products;
                    // Remove purchase from pending list
                    pendingPurchases.filter { purchase -> purchase.purchaseToken != token }
                }
            }
        }
        //
        billingClient.consumeAsync(consumeParams, consumeResponseListener)
        return true
    }

    // Non-consumables & Subscriptions

    @UsedByGodot
    suspend fun acknowledgePurchase(token: String): Boolean {
        var purchase = getPurchase(token)
        if (purchase == null) {
            Log.d(pluginName, "No purchase with token $token could be found")
            return false
        }
        if (purchase.purchaseState != PurchaseState.PURCHASED) {
            Log.d(pluginName, "Purchase currently in state ${purchase.purchaseState}")
            return false
        }
        //
        val acknowledgePurchaseParams = AcknowledgePurchaseParams.newBuilder()
            .setPurchaseToken(token)
            .build()
        //
        var billingResult = billingClient.acknowledgePurchase(acknowledgePurchaseParams)
        return billingResult.responseCode == BillingResponseCode.OK
    }
}