
namespace Lavos.Plugins
{
    sealed class LavosPlugin
    {
        readonly Godot.Object _object;

        public LavosPlugin(Godot.Object obj)
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
}