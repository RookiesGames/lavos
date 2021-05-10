using Vortico.Core.Debug;
using System.Collections.Generic;

namespace Vortico.Core.Dependency
{
    public sealed class ServiceLocator
    {
        private readonly DependencyContainer _container;

        private static ServiceLocator Instance;


        internal ServiceLocator(DependencyContainer container)
        {
            _container = container;
            Instance = this;
        }


        #region Single

        public static T Locate<T>()
        {
            T obj = Instance.LocateInternal<T>();
            Assert.IsTrue(obj != null, $"Could not locate type {typeof(T)}");
            return obj;
        }

        private T LocateInternal<T>()
        {
            var type = typeof(T);
            return (T)_container.FindOrCreateType(type);
        }

        #endregion


        #region List

        public static List<T> LocateAsList<T>()
        {
            List<T> objs = Instance.LocateAsListInternal<T>();
            Assert.IsFalse(objs.Count == 0, $"Could not locate type {typeof(T)}");
            return objs;
        }

        private List<T> LocateAsListInternal<T>()
        {
            var type = typeof(T);
            var objs = _container.FindList(type);
            var list = new List<T>(objs.Count);
            objs.ForEach(obj => list.Add((T)obj));
            return list;
        }

        #endregion
    }
}