package eu.novuloj.lavostest

import org.godotengine.godot.Godot
import org.godotengine.godot.plugin.GodotPlugin;

class LavosTest(godot: Godot) : GodotPlugin(godot) {

    private val mPluginName = "LavosTest"

    override fun getPluginName(): String = mPluginName

    fun getMethods(): List<String> = listOf(getLavosRoar().toString(),


        getLavosLife().toString(),)

    fun getLavosRoar(): String = "Roar!"
    private fun getLavosLife(): Int = 100
}