package eu.novuloj.lavostest

import androidx.annotation.NonNull
import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin
import org.godotengine.godot.plugin.UsedByGodot

class LavosTest(godot: Godot) : GodotPlugin(godot) {
    private val mPluginName = "LavosTest"

    @Override
    @NonNull
    override fun getPluginName(): String = mPluginName

    @UsedByGodot
    fun getLavosRoar(): String = "Roar!"

    @UsedByGodot
    fun getLavosLife(): Int = 100
}
