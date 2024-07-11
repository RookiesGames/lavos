using System;

namespace Lavos.Core;

public struct LazyBuilder<T> where T : class, new()
{
    readonly Func<T> _constructor;

    T _instance;
    public T Instance => _instance ??= Build();

    public LazyBuilder(Func<T> constructor = null)
    {
        _constructor = constructor;
        _instance = null;
    }

    private T Build()
    {
        return (_constructor == null)
                    ? Activator.CreateInstance<T>()
                    : _constructor.Invoke();
    }
}
