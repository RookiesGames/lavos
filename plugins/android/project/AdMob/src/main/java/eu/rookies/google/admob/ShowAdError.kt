package eu.rookies.google.admob

import com.google.android.gms.ads.AdError
import kotlinx.serialization.json.put
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject

class ShowAdError {
    companion object {
        fun toJson(id: String, error: AdError): JsonObject {
            return buildJsonObject {
                put("id", id)
                put("code", error.code)
                put("message", error.message)
            }
        }
    }
}