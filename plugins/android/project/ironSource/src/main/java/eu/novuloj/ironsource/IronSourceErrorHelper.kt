package eu.novuloj.ironsource

import com.ironsource.mediationsdk.logger.IronSourceError
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class IronSourceErrorHelper {
    companion object {
        fun toJson(error: IronSourceError): JsonObject {
            return buildJsonObject {
                putJsonObject("error") {
                    put("code", error.errorCode)
                    put("message", error.errorMessage)
                }
            }
        }
    }
}