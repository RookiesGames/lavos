using System;

namespace Lavos.Utils.Lazy;

public sealed class LazyBuilder<T> where T : class
{
    readonly Func<T> _constructor;

    T _instance;
    public T Instance => _instance ??= Build();

    public LazyBuilder(Func<T> constructor = null)
    {
        _constructor = constructor;
    }

    T Build()
    {
        return (_constructor == null)
                    ? Activator.CreateInstance<T>()
                    : _constructor.Invoke();
    }
}
