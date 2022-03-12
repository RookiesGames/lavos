using Lavos.Core.Dependency;

namespace Lavos.Utils.Lazy
{
    public struct LazyRef<T>
    {
        LazyRef(T instance = default(T))
        {
            _instance = instance;
        }

        T _instance;

        public T Ref => _instance != null ? _instance : (_instance = ServiceLocator.Locate<T>());
    }
}