package eu.rookies.google.playgames

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.util.Log
import androidx.activity.result.ActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.NonNull
import com.google.android.gms.games.AchievementsClient
import com.google.android.gms.games.FriendsResolutionRequiredException
import com.google.android.gms.games.GamesSignInClient
import com.google.android.gms.games.LeaderboardsClient
import com.google.android.gms.games.PlayGames
import com.google.android.gms.games.PlayGamesSdk;
import com.google.android.gms.games.Player
import com.google.android.gms.games.PlayerBuffer
import com.google.android.gms.games.PlayersClient
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class GooglePlayGames(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = "PlayGames"

    private var isSignedIn: Boolean = false
    private var player: Player? = null

    private var fetchFriendsStatus = FetchFriendsStatus.Completed
    private var friends: MutableList<String> = mutableListOf()

    private lateinit var gamesSignInClient: GamesSignInClient
    private lateinit var gamesPlayerClient: PlayersClient
    private lateinit var gamesAchievementsClient: AchievementsClient
    private lateinit var gamesLeaderboardsClient: LeaderboardsClient

    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        PlayGamesSdk.initialize(godot.requireContext())
        //
        gamesSignInClient = PlayGames.getGamesSignInClient(godot.requireActivity())
        gamesPlayerClient = PlayGames.getPlayersClient(godot.requireActivity())
        gamesAchievementsClient = PlayGames.getAchievementsClient(godot.requireActivity())
        gamesLeaderboardsClient = PlayGames.getLeaderboardsClient(godot.requireActivity())
        //
        gamesSignInClient.isAuthenticated.addOnCompleteListener { authResult ->
            if (!authResult.isSuccessful) {
                Log.d(
                    pluginName,
                    "Auth failed: ${authResult.exception?.message}"
                )
                return@addOnCompleteListener
            }
            //
            Log.d(
                pluginName,
                "Auth completed"
            )
            isSignedIn = authResult.result.isAuthenticated
            if (isSignedIn) {
                // Get current player
                gamesPlayerClient.currentPlayer.addOnCompleteListener { playerResult ->
                    if (playerResult.isSuccessful) {
                        player = playerResult.result
                        Log.d(pluginName, "Current player fetched")
                    } else {
                        player = null
                        Log.d(pluginName, "Failed to fetch current player")
                    }
                }
            }
        }
    }

    /////////////////
    // Auth

    @UsedByGodot
    fun signIn() = gamesSignInClient.signIn()

    @UsedByGodot
    fun isSignedIn(): Boolean = isSignedIn

    /////////////////
    // Player

    @UsedByGodot
    fun getPlayer(): String = if (player != null) {
        PlayerHelper.toJson(player!!).toString()
    } else {
        "<no_player>"
    }

    @UsedByGodot
    fun fetchFriends() {
        fetchFriendsStatus = FetchFriendsStatus.InProgress
        //
        gamesPlayerClient.loadFriends(200, false).addOnSuccessListener { data ->
            Log.d(pluginName, "Friends loaded successfully")
            val buffer: PlayerBuffer? = data.get()
            if (buffer != null) {
                buffer.forEach { friends.add(PlayerHelper.toJson(it).toString()) }
                buffer.release()
            }
            fetchFriendsStatus = FetchFriendsStatus.Completed
        }.addOnFailureListener { exception ->
            Log.d(pluginName, "Failed to load friends. ${exception.message}")
            fetchFriendsStatus = FetchFriendsStatus.Failed
            if (exception is FriendsResolutionRequiredException) {
                val pendingIntent = exception.resolution
                val activity = godot.requireActivity()
                    .registerForActivityResult(ActivityResultContracts.StartIntentSenderForResult()) { result ->
                        if (result.resultCode == Activity.RESULT_OK) {
                            Log.d(pluginName, "Friends Resolution successful")
                        } else {
                            Log.d(pluginName, "Friends resolution failed. ${result.resultCode}")
                        }
                    }
                pendingIntent.send()
            }
        }
    }

    /////////////////
    // Achievements

    @UsedByGodot
    fun unlockAchievement(id: String) = gamesAchievementsClient.unlock(id)

    @UsedByGodot
    fun incrementAchievement(id: String, amount: Int) =
        gamesAchievementsClient.increment(id, amount)

    @UsedByGodot
    fun revealAchievement(id: String) = gamesAchievementsClient.reveal(id)

    @UsedByGodot
    fun setSteps(id: String, steps: Int) = gamesAchievementsClient.setSteps(id, steps)

    @UsedByGodot
    fun showAchievements() {
        gamesAchievementsClient.achievementsIntent.addOnSuccessListener { intent ->
            val activity = godot.requireActivity()
                .registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
                    if (result.resultCode == Activity.RESULT_OK) {
                        Log.d(pluginName, "Showing achievements")
                    } else {
                        Log.d(pluginName, "Failed to show achievements. ${result.resultCode}")
                    }
                }
            activity.launch(intent)
        }
    }

    /////////////////
    // Leaderboards

    @UsedByGodot
    fun submitLeaderboardScore(id: String, score: Long) =
        gamesLeaderboardsClient.submitScore(id, score)

    @UsedByGodot
    fun showLeaderboards(id: String) {
        gamesLeaderboardsClient.getLeaderboardIntent(id).addOnSuccessListener { intent ->
            val activity = godot.requireActivity()
                .registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
                    if (result.resultCode == Activity.RESULT_OK) {
                        Log.d(pluginName, "Showing leaderboards")
                    } else {
                        Log.d(
                            pluginName,
                            "Failed to show leaderboard. ${result.resultCode}"
                        )
                    }
                }
            activity.launch(intent)
        }
    }

    /////////////////
    // Cloud save

}