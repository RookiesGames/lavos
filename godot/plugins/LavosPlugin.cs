
namespace Lavos.Plugins
{
    sealed class LavosPlugin
    {
        readonly Godot.Object _object;

        public LavosPlugin(Godot.Object obj)
        {
            _object = obj;
        }

        public void CallVoid(string method, params object[] args)
        {
            _object.Call(method, args);
        }

        public T Call<T>(string method, params object[] args)
        {
            return (T)_object.Call(method, args);
        }
    }
}