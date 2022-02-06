
namespace Lavos.Plugins
{
    public class LavosPlugin : Godot.Object
    {
        public void CallVoid(string method, params object[] args)
        {
            Call(method, args);
        }

        public T Call<T>(string method, params object[] args)
        {
            return (T)Call(method, args);
        }
    }
}