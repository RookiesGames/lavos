package eu.rookies.google.billing

import com.android.billingclient.api.ProductDetails
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.add
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonArray
import kotlinx.serialization.json.putJsonObject

class ProductHelper {
    companion object {
        fun toJson(product: ProductDetails): JsonObject {
            return buildJsonObject {
                put("id", product.productId)
                put("name", product.name)
                put("description", product.description)
                put("type", product.productType)
                if (product.oneTimePurchaseOfferDetails != null) {
                    val details = product.oneTimePurchaseOfferDetails!!
                    putJsonObject("price") {
                        put("formatted", details.formattedPrice)
                        put("currency", details.priceCurrencyCode)
                        put("amount", details.priceAmountMicros)
                    }
                }
                if (product.subscriptionOfferDetails != null) {
                    val details = product.subscriptionOfferDetails!!
                    putJsonArray("subscriptionsOfferDetails") {
                        for (detail in details) {
                            add(
                                buildJsonObject {
                                    put("id", detail.basePlanId)
                                    putJsonObject("offer") {
                                        put("id", detail.offerId)
                                        put("token", detail.offerToken)
                                        putJsonArray("tags") {
                                            for (tag in detail.offerTags) {
                                                add(tag)
                                            }
                                        }
                                    }
                                    putJsonArray("phases") {
                                        for (phase in detail.pricingPhases.pricingPhaseList) {
                                            val entry = buildJsonObject {
                                                put("mode", phase.recurrenceMode)
                                                putJsonObject("billing") {
                                                    put("period", phase.billingPeriod)
                                                    put("cycle", phase.billingCycleCount)
                                                }
                                                putJsonObject("price") {
                                                    put("formatted", phase.formattedPrice)
                                                    put("currency", phase.priceCurrencyCode)
                                                    put("amount", phase.priceAmountMicros)
                                                }
                                            }
                                            add(entry)
                                        }
                                    }
                                }
                            )
                        }
                    }
                }
            }
        }
    }
}