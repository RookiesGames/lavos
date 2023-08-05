package eu.rookies.google.admob.interstitials

import com.google.android.gms.ads.interstitial.InterstitialAd
import kotlinx.serialization.json.put
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.putJsonObject

class InterstitialAdHelper {
    companion object {
        fun toJson(ad: InterstitialAd): JsonObject {
            return buildJsonObject {
                putJsonObject("info") {
                    put("ad_unit", ad.adUnitId)
                }
            }
        }
    }
}