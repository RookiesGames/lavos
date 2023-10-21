package eu.rookies.google.admob.banner

import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put

class BannerAdInfo {
    companion object {
        fun toJson(id: String): JsonObject {
            return buildJsonObject {
                put("id", id)
            }
        }
    }
}