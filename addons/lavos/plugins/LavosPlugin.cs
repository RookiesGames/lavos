using Godot;

namespace Lavos.Plugins;

sealed class LavosPlugin
{
    readonly GodotObject GO;

    public LavosPlugin(GodotObject obj)
    {
        GO = obj;
    }

    public void CallVoid(string method, params Variant[] args) => GO.Call(method, args);
    public bool CallBool(string method, params Variant[] args) => GO.Call(method, args).AsBool();
    public int CallInt(string method, params Variant[] args) => GO.Call(method, args).AsInt32();
    public float CallFloat(string method, params Variant[] args) => GO.Call(method, args).AsFloat();
    public string CallString(string method, params Variant[] args) => GO.Call(method, args).AsString();
    public string[] CallStringArray(string method, params Variant[] args) => GO.Call(method, args).AsStringArray();
}
