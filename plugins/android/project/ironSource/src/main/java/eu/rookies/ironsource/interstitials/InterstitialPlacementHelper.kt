package eu.rookies.ironsource.interstitials

import com.ironsource.mediationsdk.model.InterstitialPlacement
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class InterstitialPlacementHelper {
    companion object {
        fun toJson(placement: InterstitialPlacement): JsonObject {
            return buildJsonObject {
                putJsonObject("placement") {
                    put("id", placement.placementId)
                    put("name", placement.placementName)
                }
            }
        }
    }
}