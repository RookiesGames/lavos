using Lavos.Dependency;

namespace Lavos.Utils.Lazy;

public struct LazyRef<T> where T : IService
{
    LazyRef(T instance = default)
    {
        _instance = instance;
    }

    T _instance;

    public T Ref => _instance != null ? _instance : (_instance = ServiceLocator.Locate<T>());
}
