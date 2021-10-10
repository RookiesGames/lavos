using System;

namespace Vortico.Utils.Lazy
{
    public sealed class LazyBuilder<T> where T : class
    {
        T _instance = null;
        Func<T> _constructor;
        Action _action;

        public T Get
        {
            get
            {
                if (_instance == null) { Build(); }
                return _instance;
            }
        }

        public LazyBuilder(Func<T> constructor)
        {
            _constructor = constructor;
        }

        public void Build()
        {
            if (_constructor == null)
            {
                _instance = Activator.CreateInstance<T>();
            }
            else
            {
                _instance = _constructor.Invoke();
            }
        }
    }
}