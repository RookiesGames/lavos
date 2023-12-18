package eu.rookies.deviceinfo

import android.content.pm.PackageInfo
import android.content.pm.PackageManager
import android.os.Build
import androidx.annotation.NonNull
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class DeviceInfo(godot: Godot) : GodotPlugin(godot) {
    private val pluginName = BuildConfig.GODOT_PLUGIN_NAME

    @NonNull
    override fun getPluginName(): String = pluginName

    @UsedByGodot
    fun getPlatform(): String = "Android"

    @UsedByGodot
    fun getOSVersion(): String = Build.VERSION.SDK_INT.toString()

    @UsedByGodot
    fun getDeviceName(): String = "${Build.MANUFACTURER}.${Build.MODEL}"

    @UsedByGodot
    fun getVersionName(): String {
        return try {
            val pInfo: PackageInfo =
                activity!!.packageManager.getPackageInfo(activity!!.packageName, 0)
            pInfo.versionName
        } catch (e: PackageManager.NameNotFoundException) {
            ""
        }
    }

    @UsedByGodot
    fun getVersionCode(): String {
        return try {
            val pInfo: PackageInfo =
                activity!!.packageManager.getPackageInfo(activity!!.packageName, 0)
            pInfo.versionCode.toString()
        } catch (e: PackageManager.NameNotFoundException) {
            ""
        }
    }
}