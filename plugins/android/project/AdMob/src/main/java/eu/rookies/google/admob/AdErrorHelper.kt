package eu.rookies.google.admob

import com.google.android.gms.ads.AdError
import kotlinx.serialization.json.put
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.putJsonObject

class AdErrorHelper {
    companion object {
        fun toJson(error: AdError): JsonObject {
            return buildJsonObject {
                putJsonObject("error") {
                    put("code", error.code)
                    put("message", error.message)
                }
            }
        }
    }
}