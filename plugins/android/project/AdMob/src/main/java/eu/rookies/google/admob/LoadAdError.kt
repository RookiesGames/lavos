package eu.rookies.google.admob

import com.google.android.gms.ads.LoadAdError
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put

class LoadAdError {
    companion object {
        fun toJson(id: String, error: LoadAdError): JsonObject {
            return buildJsonObject {
                put("id", id)
                put("code", error.code)
                put("message", error.message)
            }
        }
    }
}