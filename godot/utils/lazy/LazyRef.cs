using Lavos.Core.Dependency;

namespace Lavos.Utils.Lazy
{
    public sealed class LazyRef<T> where T : class
    {
        T _instance = null;

        public T Get => _instance ?? (_instance = ServiceLocator.Locate<T>());
    }
}