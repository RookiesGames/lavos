package eu.rookies.ironsource

import com.ironsource.mediationsdk.adunit.adapter.utility.AdInfo
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class AdInfoHelper {
    companion object {
        fun toJson(adInfo: AdInfo): JsonObject {
            return buildJsonObject {
                putJsonObject("ad_info") {
                    put("auction_id", adInfo.auctionId)
                    put("ad_unit", adInfo.adUnit)
                    put("country", adInfo.country)
                    put("ab", adInfo.ab)
                    put("segment_name", adInfo.segmentName)
                    put("ad_network", adInfo.adNetwork)
                    put("instance_name", adInfo.instanceName)
                    put("instance_id", adInfo.instanceId)
                    put("precision", adInfo.precision)
                    put("revenue", adInfo.revenue)
                    put("lifetime_revenue", adInfo.lifetimeRevenue)
                    put("encrypted_cpm", adInfo.encryptedCPM)
                }
            }
        }
    }
}