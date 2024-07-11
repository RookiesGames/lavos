using Lavos.Core;

namespace Lavos.Utils;

public class Singleton<T> where T : class, new()
{
    static readonly LazyBuilder<T> _builder = new();
    private static T _instance = null;
    public static T Instance => _instance ??= _builder.Instance;
}
