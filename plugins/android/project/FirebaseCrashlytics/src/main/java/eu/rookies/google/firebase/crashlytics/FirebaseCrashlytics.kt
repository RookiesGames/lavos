package eu.rookies.google.firebase.crashlytics

import android.app.Activity
import android.view.View
import androidx.annotation.NonNull
import androidx.annotation.Nullable
import com.google.firebase.FirebaseApp
import com.google.firebase.crashlytics.FirebaseCrashlytics
import com.google.firebase.crashlytics.ktx.crashlytics
import com.google.firebase.ktx.Firebase
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class FirebaseCrashlytics(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = BuildConfig.GODOT_PLUGIN_NAME
    private lateinit var crashlytics: FirebaseCrashlytics

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        FirebaseApp.initializeApp(activity!!.applicationContext)
        crashlytics = Firebase.crashlytics
    }

    @UsedByGodot
    fun checkForUnsentReports(): Boolean = false // TODO

    @UsedByGodot
    fun deleteUnsentReports() = crashlytics.deleteUnsentReports()

    @UsedByGodot
    fun didCrashOnPreviousExecution(): Boolean = crashlytics.didCrashOnPreviousExecution()

    @UsedByGodot
    fun log(message: String) = crashlytics.log(message)

    @UsedByGodot
    fun recordException(message: String) {
        val throwable: Throwable = Throwable(message)
        crashlytics.recordException(throwable);
    }

    @UsedByGodot
    fun sendUnsentReports() = crashlytics.sendUnsentReports()

    @UsedByGodot
    fun setCrashlyticsCollectionEnabled(enabled: Boolean) =
        crashlytics.setCrashlyticsCollectionEnabled(enabled)

    @UsedByGodot
    fun setCustomKeyB(key: String, value: Boolean) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKeyF(key: String, value: Float) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKeyI(key: String, value: Int) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKeyS(key: String, value: String) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setUserId(identifier: String) = crashlytics.setUserId(identifier)

    @UsedByGodot
    fun crash(): Boolean = throw RuntimeException("From $pluginName")
}
