package eu.rookies.google.playgames

import com.google.android.gms.games.stats.PlayerStats
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.putJsonObject

class StatsHelper {
    companion object {
        fun toJson(stats: PlayerStats): JsonObject {
            return buildJsonObject {
                putJsonObject("stats") {
                    put("last_played", stats.daysSinceLastPlayed)
                    putJsonObject("session") {
                        put("count", stats.numberOfSessions)
                        put("average", stats.averageSessionLength)
                        put("percentile", stats.sessionPercentile)
                    }
                    putJsonObject("spending") {
                        put("count", stats.numberOfPurchases)
                        put("percentile", stats.spendPercentile)
                    }
                }
            }
        }
    }
}