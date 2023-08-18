package eu.rookies.google.playgames

import android.annotation.SuppressLint
import com.google.android.gms.games.Player
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class PlayerHelper {
    companion object {
        @SuppressLint("VisibleForTests")
        fun toJson(player: Player): JsonObject {
            return buildJsonObject {
                putJsonObject("player") {
                    put("id", player.playerId)
                    put("name", player.displayName)
                    put("title", player.title)
                    putJsonObject("level_info") {
                        put("is_max_level", player.levelInfo?.isMaxLevel)
                        put("total_xp", player.levelInfo?.currentXpTotal)
                        put("last_level_timestamp", player.levelInfo?.lastLevelUpTimestamp)
                        putJsonObject("current") {
                            put("level", player.levelInfo?.currentLevel?.levelNumber)
                            put("min", player.levelInfo?.currentLevel?.minXp)
                            put("max", player.levelInfo?.currentLevel?.maxXp)
                        }
                        putJsonObject("next") {
                            put("level", player.levelInfo?.nextLevel?.levelNumber)
                            put("min", player.levelInfo?.nextLevel?.minXp)
                            put("max", player.levelInfo?.nextLevel?.maxXp)
                        }
                    }
                    putJsonObject("info") {
                        put("friendship", player.currentPlayerInfo?.friendsListVisibilityStatus)
                    }
                    putJsonObject("friendship") {
                        put("status", player.relationshipInfo?.friendStatus)
                    }
                }
            }
        }
    }
}