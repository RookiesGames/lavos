using Lavos.Dependency;

namespace Lavos.Core;

public struct LazyRef<T> where T : IService
{
    T _instance;
    public T Ref => _instance ??= ServiceLocator.Locate<T>();
}
