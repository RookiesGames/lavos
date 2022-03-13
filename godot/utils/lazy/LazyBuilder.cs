using System;

namespace Lavos.Utils.Lazy
{
    public sealed class LazyBuilder<T> where T : class
    {
        T _instance = null;
        Func<T> _constructor;

        public T Get => _instance ?? (_instance = Build());

        public LazyBuilder(Func<T> constructor = null)
        {
            _constructor = constructor;
        }

        T Build()
        {
            return _instance =
                        (_constructor == null)
                            ? Activator.CreateInstance<T>()
                            : _constructor.Invoke();
        }
    }
}