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
                putJsonObject("product") {
                    put("id", product.productId)
                    put("name", product.name)
                    put("description", product.description)
                    put("type", product.productType)
                    if (product.oneTimePurchaseOfferDetails != null) {
                        toJson(product.oneTimePurchaseOfferDetails!!)
                    }
                    if (product.subscriptionOfferDetails != null) {
                        toJson(product.subscriptionOfferDetails!!)
                    }
                }
            }
        }

        private fun toJson(details: ProductDetails.OneTimePurchaseOfferDetails): JsonObject {
            return buildJsonObject {
                putJsonObject("price") {
                    put("formatted", details.formattedPrice)
                    put("currency", details.priceCurrencyCode)
                    put("amount", details.priceAmountMicros)
                }
            }
        }

        private fun toJson(details: List<ProductDetails.SubscriptionOfferDetails>): JsonObject {
            return buildJsonObject {
                putJsonArray("subscriptions") {
                    for (detail in details) {
                        add(toJson(detail))
                    }
                }
            }
        }

        private fun toJson(details: ProductDetails.SubscriptionOfferDetails): JsonObject {
            return buildJsonObject {
                putJsonObject("subscription") {
                    put("plan_id", details.basePlanId)
                    putJsonObject("offer") {
                        put("id", details.offerId)
                        put("token", details.offerToken)
                        putJsonArray("tags") {
                            for (tag in details.offerTags) {
                                add(tag)
                            }
                        }
                    }
                    putJsonArray("phases") {
                        for (phase in details.pricingPhases.pricingPhaseList) {
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
            }
        }
    }
}