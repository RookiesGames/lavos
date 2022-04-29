using System;

namespace Lavos.Utils.Lazy
{
    public sealed class LazyBuilder<T> where T : class
    {
        Func<T> _constructor = null;

        T _instance = null;
        public T Instance => _instance ?? (_instance = Build());


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
}