using Lavos.Dependency;

namespace Lavos.Utils.Lazy;

public struct LazyRef<T> where T : IService
{
    T _instance;
    public T Ref => _instance ??= ServiceLocator.Locate<T>();
}
