package eu.rookies.ironsource.rewardedads

import com.ironsource.mediationsdk.model.Placement
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class PlacementHelper {
    companion object {
        fun toJson(placement: Placement): JsonObject {
            return buildJsonObject {
                putJsonObject("placement") {
                    put("id", placement.placementId)
                    put("name", placement.placementName)
                    putJsonObject("reward") {
                        put("name", placement.rewardName)
                        put("amount", placement.rewardAmount)
                    }
                }
            }
        }
    }
}