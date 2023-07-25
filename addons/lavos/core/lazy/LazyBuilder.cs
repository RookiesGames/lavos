using System;

namespace Lavos.Core;

public struct LazyBuilder<T> where T : class
{
    readonly Func<T> _constructor;

    T _instance;
    public T Instance => _instance ??= Build();

    public LazyBuilder(Func<T> constructor = null)
    {
        _constructor = constructor;
        _instance = null;
    }

    readonly T Build()
    {
        return (_constructor == null)
                    ? Activator.CreateInstance<T>()
                    : _constructor.Invoke();
    }
}
