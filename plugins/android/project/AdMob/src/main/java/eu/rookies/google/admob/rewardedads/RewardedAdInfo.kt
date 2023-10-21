package eu.rookies.google.admob.rewardedads

import com.google.android.gms.ads.rewarded.RewardedAd
import kotlinx.serialization.json.put
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.putJsonObject

class RewardedAdInfo {
    companion object {
        fun toJson(ad: RewardedAd): JsonObject {
            return buildJsonObject {
                put("id", ad.adUnitId)
                putJsonObject("reward") {
                    put("type", ad.rewardItem.type)
                    put("amount", ad.rewardItem.amount)
                }
            }
        }
    }
}