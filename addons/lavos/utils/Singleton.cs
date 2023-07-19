using Godot;
using Lavos.Core;

namespace Lavos.Utils;

public class Singleton<T> where T : class
{
    static readonly LazyBuilder<T> _builder;
    public static T Instance => _builder.Instance;
}