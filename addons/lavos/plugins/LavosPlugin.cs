using Godot;

namespace Lavos.Plugins;

sealed class LavosPlugin
{
    readonly GodotObject _object;

    public LavosPlugin(GodotObject obj)
    {
        _object = obj;
    }

    public void CallVoid(string method, params Godot.Variant[] args)
    {
        _object.Call(method, args);
    }

    public bool CallBool(string method, params Godot.Variant[] args)
    {
        return _object.Call(method, args).AsBool();
    }
}
