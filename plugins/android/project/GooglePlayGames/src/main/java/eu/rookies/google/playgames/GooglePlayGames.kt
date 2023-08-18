package eu.rookies.google.playgames

import android.content.Intent
import android.util.Log
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.drive.Drive.SCOPE_APPFOLDER
import com.google.android.gms.games.AchievementsClient
import com.google.android.gms.games.FriendsResolutionRequiredException
import com.google.android.gms.games.GamesSignInClient
import com.google.android.gms.games.LeaderboardsClient
import com.google.android.gms.games.PlayGames
import com.google.android.gms.games.PlayGamesSdk
import com.google.android.gms.games.Player
import com.google.android.gms.games.PlayerBuffer
import com.google.android.gms.games.PlayerStatsClient
import com.google.android.gms.games.PlayersClient
import com.google.android.gms.games.SnapshotsClient
import com.google.android.gms.games.SnapshotsClient.RESOLUTION_POLICY_MOST_RECENTLY_MODIFIED
import com.google.android.gms.games.snapshot.SnapshotMetadata
import com.google.android.gms.games.snapshot.SnapshotMetadataChange
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot
import java.math.BigInteger
import java.util.Random


class GooglePlayGames(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = GooglePlayGames::class.java.simpleName

    private var isSignedIn: Boolean = false
    private var player: Player? = null
    private var playerStats: String = ""

    private var fetchFriendsStatus = FetchFriendsStatus.Completed
    private var friends: MutableList<String> = mutableListOf()

    private var savingSavedGameStatus = SaveGameStatus.None
    private var loadingSavedGameStatus = LoadGameStatus.None
    private var currentSavedGame: String = ""

    private lateinit var gamesSignInClient: GamesSignInClient
    private lateinit var gamesPlayerClient: PlayersClient
    private lateinit var gamesAchievementsClient: AchievementsClient
    private lateinit var gamesLeaderboardsClient: LeaderboardsClient
    private lateinit var gamesPlayerStatsClient: PlayerStatsClient
    private lateinit var gamesSnapshotsClient: SnapshotsClient

    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        PlayGamesSdk.initialize(godot.getActivity()!!.applicationContext)
        //
        gamesSignInClient = PlayGames.getGamesSignInClient(godot.getActivity()!!)
        gamesPlayerClient = PlayGames.getPlayersClient(godot.getActivity()!!)
        gamesAchievementsClient = PlayGames.getAchievementsClient(godot.getActivity()!!)
        gamesLeaderboardsClient = PlayGames.getLeaderboardsClient(godot.getActivity()!!)
        gamesPlayerStatsClient = PlayGames.getPlayerStatsClient(godot.getActivity()!!)
        gamesSnapshotsClient = PlayGames.getSnapshotsClient(godot.getActivity()!!)
        //
        registerForSignInChanges { isSignedIn ->
            if (isSignedIn) {
                loadCurrentPlayer {
                    loadPlayerStats()
                }
                // Cloud saves
                cloudSignIn()
            }
        }
    }

    /////////////////
    // Auth

    @UsedByGodot
    fun signIn() = gamesSignInClient.signIn()

    @UsedByGodot
    fun isSignedIn(): Boolean = isSignedIn

    private fun registerForSignInChanges(action: (signedIn: Boolean) -> Unit) {
        gamesSignInClient.isAuthenticated.addOnCompleteListener { authResult ->
            if (!authResult.isSuccessful) {
                Log.d(pluginName, "Auth failed")
                if (authResult.exception != null) {
                    Log.e(pluginName, "${authResult.exception!!.message}")
                }
                return@addOnCompleteListener
            }
            //
            Log.d(pluginName, "Auth completed")
            isSignedIn = authResult.result.isAuthenticated
            action(isSignedIn)
        }
    }

    /////////////////
    // Player

    @UsedByGodot
    fun getPlayer(): String = if (player != null) {
        PlayerHelper.toJson(player!!).toString()
    } else {
        ""
    }

    private fun loadCurrentPlayer(action: () -> Unit) {
        gamesPlayerClient.currentPlayer.addOnCompleteListener { playerResult ->
            if (playerResult.isSuccessful) {
                player = playerResult.result
                Log.d(pluginName, "Current player fetched")
                //
                action()
            } else {
                player = null
                Log.d(pluginName, "Failed to fetch current player")
            }
        }
    }

    /////////////////
    // Friends

    @UsedByGodot
    fun getFetchFriendsStatus(): Int = fetchFriendsStatus.id

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
                val errorRequestCode = 34404
                val pendingIntent = exception.resolution
                godot.getActivity()!!.startIntentSenderForResult(
                    pendingIntent.intentSender,
                    /* requestCode */ errorRequestCode,
                    /* fillInIntent */ null,
                    /* flagsMask */ 0,
                    /* flagsValues */ 0,
                    /* extraFlags */ 0,
                    /* options */ null
                );
            }
        }
    }

    @UsedByGodot
    fun compareToFriend(id: String) {
        val friendsRequestCode = 7413
        gamesPlayerClient.getCompareProfileIntent(id).addOnSuccessListener { intent ->
            godot.getActivity()!!.startActivityForResult(intent, friendsRequestCode)
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
        val achievementsRequestCode = 413
        gamesAchievementsClient.achievementsIntent.addOnSuccessListener { intent ->
            godot.getActivity()!!.startActivityForResult(intent, achievementsRequestCode)
        }
    }

    /////////////////
    // Leaderboards

    @UsedByGodot
    fun submitLeaderboardScore(id: String, score: Long) =
        gamesLeaderboardsClient.submitScore(id, score)

    @UsedByGodot
    fun showLeaderboards(id: String) {
        val leaderboardsRequestCode = 13463
        gamesLeaderboardsClient.getLeaderboardIntent(id).addOnSuccessListener { intent ->
            godot.getActivity()!!.startActivityForResult(intent, leaderboardsRequestCode)
        }
    }

    /////////////////
    // Stats

    @UsedByGodot
    fun getPlayerStats(): String = playerStats

    private fun loadPlayerStats() {
        gamesPlayerStatsClient.loadPlayerStats(true).addOnCompleteListener {
            if (!it.isSuccessful) {
                Log.d(pluginName, "Failed to get player stats")
                return@addOnCompleteListener
            }
            //
            val stats = it.result.get()
            if (stats == null) {
                Log.e(pluginName, "No stats could be retrieved")
                return@addOnCompleteListener
            }
            //
            playerStats = StatsHelper.toJson(stats).toString()
        }
    }

    /////////////////
    // Cloud save

    private fun cloudSignIn() {
        val options: GoogleSignInOptions =
            GoogleSignInOptions.Builder().requestScopes(SCOPE_APPFOLDER).build()

        val client = GoogleSignIn.getClient(godot.getActivity()!!, options)
        client.silentSignIn().addOnCompleteListener { task ->
            if (task.isSuccessful) {
                Log.d(pluginName, "Successfully connected to cloud storage")
            } else {
                Log.d(pluginName, "Failed to connect to cloud storage")
            }
        }
    }

    override fun onMainActivityResult(requestCode: Int, resultCode: Int, intent: Intent) {
        if (intent.hasExtra(SnapshotsClient.EXTRA_SNAPSHOT_METADATA)) {
            val metadata: SnapshotMetadata =
                intent.getParcelableExtra(SnapshotsClient.EXTRA_SNAPSHOT_METADATA)!!
            var name = metadata.uniqueName
            Log.d(pluginName, "$name cloud save selected")
            // TODO
        } else if (intent.hasExtra(SnapshotsClient.EXTRA_SNAPSHOT_NEW)) {
            val unique: String = BigInteger(281, Random()).toString(13)
            var name = "snapshotTemp-$unique"
            Log.d(pluginName, "$name cloud save created")
        }
    }

    @UsedByGodot
    fun showCloudSaves() {
        gamesSnapshotsClient
            .getSelectSnapshotIntent("Saved games", true, true, 5)
            .addOnSuccessListener { intent ->
                val cloudRequestCode = 610
                godot.getActivity()!!.startActivityForResult(intent, cloudRequestCode)
            }
    }

    @UsedByGodot
    fun getSaveSavedGameStatus(): Int = savingSavedGameStatus.id

    @UsedByGodot
    fun saveGame(file: String, data: String) {
        savingSavedGameStatus = SaveGameStatus.InProgress
        //
        gamesSnapshotsClient.open(file, true, RESOLUTION_POLICY_MOST_RECENTLY_MODIFIED)
            .addOnFailureListener { exception ->
                Log.d(pluginName, "Failed to open $file. ${exception.message}")
                savingSavedGameStatus = SaveGameStatus.Error
            }
            .addOnCanceledListener {
                Log.d(pluginName, "Canceled opening of file $file")
                savingSavedGameStatus = SaveGameStatus.Error
            }
            .addOnSuccessListener { result ->
                if (result.data == null) {
                    Log.d(pluginName, "No data found in snapshot")
                    savingSavedGameStatus = SaveGameStatus.Error
                    return@addOnSuccessListener
                }
                //
                val snapshot = result.data!!
                snapshot.snapshotContents.writeBytes(data.toByteArray(Charsets.UTF_8))
                //
                val metadata = SnapshotMetadataChange.Builder().build()
                //
                gamesSnapshotsClient.commitAndClose(snapshot, metadata)
                    .addOnCompleteListener { task ->
                        if (task.isSuccessful) {
                            Log.d(pluginName, "Cloud save successful")
                            savingSavedGameStatus = SaveGameStatus.Completed
                        } else {
                            Log.d(pluginName, "Cloud save failed")
                            savingSavedGameStatus = SaveGameStatus.Error
                        }
                    }
            }
    }

    @UsedByGodot
    fun getLoadingSavedGameStatus(): Int = loadingSavedGameStatus.id

    @UsedByGodot
    fun getCurrentSavedGame(): String = currentSavedGame

    @UsedByGodot
    fun loadGame(file: String) {
        currentSavedGame = ""
        loadingSavedGameStatus = LoadGameStatus.InProgress
        //
        gamesSnapshotsClient
            .open(file, true, RESOLUTION_POLICY_MOST_RECENTLY_MODIFIED)
            .addOnFailureListener { exception ->
                Log.d(pluginName, "Failed to load file $file")
                loadingSavedGameStatus = LoadGameStatus.Error
            }
            .addOnCanceledListener {
                Log.d(pluginName, "Canceled loading of file $file")
                loadingSavedGameStatus = LoadGameStatus.Error
            }
            .addOnSuccessListener { result ->
                if (result.data == null) {
                    Log.d(pluginName, "No data found in snapshot")
                    loadingSavedGameStatus = LoadGameStatus.Error
                    return@addOnSuccessListener
                }
                //
                val snapshot = result.data!!
                val data = snapshot.snapshotContents.readFully()
                currentSavedGame = data.toString(Charsets.UTF_8)
                loadingSavedGameStatus = LoadGameStatus.Completed
            }
    }
}