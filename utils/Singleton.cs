using Godot;

namespace Vortico.Utils
{
    public class Singleton<T>
    {
        protected static T _instance;
        public static T Instance => _instance;
    }
}