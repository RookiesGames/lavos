using Godot;
using Lavos.Utils.Lazy;

namespace Lavos.Utils;

public class Singleton<T> where T : class
{
    static LazyBuilder<T> _builder = new LazyBuilder<T>();
    public static T Instance => _builder.Instance;
}
