package eu.novuloj.firebasecrashlytics

import android.app.Activity
import android.view.View
import androidx.annotation.NonNull
import androidx.annotation.Nullable
import com.google.firebase.crashlytics.FirebaseCrashlytics
import com.google.firebase.crashlytics.ktx.crashlytics
import com.google.firebase.ktx.Firebase
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class FirebaseCrashlytics(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = "FirebaseCrashlytics"
    private lateinit var crashlytics: FirebaseCrashlytics

    @Nullable
    override fun onMainCreate(activity: Activity): View? {
        crashlytics = Firebase.crashlytics;
        return null;
    }

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun checkForUnsentReports(): Boolean {
        return false
    }

    @UsedByGodot
    fun deleteUnsentReports() = crashlytics.deleteUnsentReports()

    @UsedByGodot
    fun didCrashOnPreviousExecution() : Boolean = crashlytics.didCrashOnPreviousExecution()

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
    fun setCrashlyticsCollectionEnabled(enabled: Boolean) = crashlytics.setCrashlyticsCollectionEnabled(enabled)

    @UsedByGodot
    fun setCustomKey(key: String, value: Boolean) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKey(key: String, value: Double) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKey(key: String, value: Long) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setCustomKey(key: String, value: String) = crashlytics.setCustomKey(key, value)

    @UsedByGodot
    fun setUserId(identifier: String) = crashlytics.setUserId(identifier)
}
