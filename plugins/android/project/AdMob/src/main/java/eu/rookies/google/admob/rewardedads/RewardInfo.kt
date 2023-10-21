package eu.rookies.google.admob.rewardedads

import com.google.android.gms.ads.rewarded.RewardItem
import kotlinx.serialization.json.put
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject

class RewardInfo {
    companion object {
        fun toJson(item: RewardItem): JsonObject {
            return buildJsonObject {
                put("type", item.type)
                put("amount", item.amount)
            }
        }
    }
}