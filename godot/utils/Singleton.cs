using Godot;
using Lavos.Utils.Lazy;

namespace Lavos.Utils;

public class Singleton<T> where T : class
{
    static readonly LazyBuilder<T> _builder = new();
    public static T Instance => _builder.Instance;
}
