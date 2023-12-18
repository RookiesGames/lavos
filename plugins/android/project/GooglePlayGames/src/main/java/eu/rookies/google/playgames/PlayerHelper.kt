package eu.rookies.google.playgames

import android.annotation.SuppressLint
import com.google.android.gms.games.Player
import com.google.android.gms.games.Player.FriendsListVisibilityStatus
import com.google.android.gms.games.Player.PlayerFriendStatus
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put
import kotlinx.serialization.json.putJsonObject

class PlayerHelper {
    companion object {
        @SuppressLint("VisibleForTests")
        fun toJson(player: Player): JsonObject {
            return buildJsonObject {
                put("id", player.playerId)
                put("name", player.displayName)
                put("title", player.title)
                putJsonObject("levelInfo") {
                    put("isMaxLevel", player.levelInfo?.isMaxLevel)
                    put("totalXP", player.levelInfo?.currentXpTotal)
                    //put("last_level_timestamp", player.levelInfo?.lastLevelUpTimestamp)
                    putJsonObject("current") {
                        put("number", player.levelInfo?.currentLevel?.levelNumber)
                        put("minXP", player.levelInfo?.currentLevel?.minXp)
                        put("maxXP", player.levelInfo?.currentLevel?.maxXp)
                    }
                    putJsonObject("next") {
                        put("number", player.levelInfo?.nextLevel?.levelNumber)
                        put("minXP", player.levelInfo?.nextLevel?.minXp)
                        put("maxXP", player.levelInfo?.nextLevel?.maxXp)
                    }
                }
                put("friendStatus", getFriendStatus(player.relationshipInfo?.friendStatus))
                put(
                    "friendsListVisibilityStatus",
                    getFriendsListVisibilityStatus(player.currentPlayerInfo?.friendsListVisibilityStatus)
                )
            }
        }

        @SuppressLint("VisibleForTests")
        private fun getFriendStatus(status: Int?): Int {
            val s = when (status) {
                PlayerFriendStatus.FRIEND -> 1
                PlayerFriendStatus.NO_RELATIONSHIP -> 2
                PlayerFriendStatus.UNKNOWN -> 0
                else -> 0
            }
            return s
        }

        @SuppressLint("VisibleForTests")
        private fun getFriendsListVisibilityStatus(status: Int?): Int {
            val s = when (status) {
                FriendsListVisibilityStatus.VISIBLE -> 1
                FriendsListVisibilityStatus.FEATURE_UNAVAILABLE -> 2
                FriendsListVisibilityStatus.REQUEST_REQUIRED -> 2
                FriendsListVisibilityStatus.UNKNOWN -> 0
                else -> 0
            }
            return s
        }
    }
}