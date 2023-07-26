using Godot;

namespace Lavos.Plugins;

sealed class LavosPlugin
{
    readonly GodotObject _object;

    public LavosPlugin(GodotObject obj)
    {
        _object = obj;
    }

    public void CallVoid(string method, params Variant[] args)
    {
        _object.Call(method, args);
    }

    public bool CallBool(string method, params Variant[] args)
    {
        return _object.Call(method, args).AsBool();
    }

    public string CallString(string method, params Variant[] args)
    {
        return _object.Call(method, args).AsString();
    }

    public string[] CallStringArray(string method, params Variant[] args)
    {
        return _object.Call(method, args).AsStringArray();
    }
}
