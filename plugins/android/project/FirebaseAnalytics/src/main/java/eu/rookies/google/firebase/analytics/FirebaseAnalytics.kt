package eu.rookies.google.firebase.analytics

import android.os.Bundle
import androidx.annotation.NonNull
import com.google.firebase.FirebaseApp
import com.google.firebase.analytics.FirebaseAnalytics
import com.google.firebase.analytics.ktx.analytics
import com.google.firebase.ktx.Firebase
import org.godotengine.godot.Dictionary
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot
import org.json.JSONObject

class FirebaseAnalytics(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = BuildConfig.GODOT_PLUGIN_NAME
    private lateinit var analytics: FirebaseAnalytics

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun init() {
        FirebaseApp.initializeApp(activity!!.applicationContext)
        analytics = Firebase.analytics
    }

    @UsedByGodot
    fun logEvent(name: String, params: Dictionary) {
        val bundle: Bundle = dicToBundle(params)
        analytics.logEvent(name, bundle)
    }

    @UsedByGodot
    fun resetAnalyticsData() = analytics.resetAnalyticsData()

    @UsedByGodot
    fun setAnalyticsCollectionEnabled(enabled: Boolean) =
        analytics.setAnalyticsCollectionEnabled(enabled)

    @UsedByGodot
    fun setDefaultEventParameters(params: Dictionary) {
        val bundle: Bundle = dicToBundle(params)
        analytics.setDefaultEventParameters(bundle)
    }

    @UsedByGodot
    fun setUserId(id: String) = analytics.setUserId(id)

    @UsedByGodot
    fun setUserProperty(name: String, value: String) =
        analytics.setUserProperty(name, value)

    private fun dicToBundle(dictionary: Dictionary): Bundle {
        val bundle: Bundle = Bundle(dictionary.size)
        dictionary.forEach {
            bundle.putString(it.key, it.value as String)
        }
        return bundle
    }
}
