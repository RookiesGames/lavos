using Lavos.Core.Dependency;

namespace Lavos.Utils.Lazy
{
    public sealed class LazyRef<T>
    {
        T _instance = default(T);

        public T Ref => _instance != null ? _instance : (_instance = ServiceLocator.Locate<T>());
    }
}