using Godot;
using Vortico.Core.Debug;
using System.Collections.Generic;

namespace Vortico.Core.Dependency
{
    public sealed class ServiceLocator : Node
    {
        public const string Path = "/root/ServiceLocator";

        private DependencyContainer _container;
        private static ServiceLocator _instance;


        #region Constructor

        private ServiceLocator()
        {
            _instance = this;
        }

        #endregion


        #region Node

        public override void _Ready()
        {
            _container = GetNode<DependencyContainer>(DependencyContainer.Path);
        }

        #endregion


        #region Single

        public static T Locate<T>()
        {
            T obj = _instance.LocateInternal<T>();
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
            List<T> objs = _instance.LocateAsListInternal<T>();
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


        public static void Register<I, C>(C instance) where C : I
        {
            _instance.RegisterInternal<I, C>(instance);
        }

        private void RegisterInternal<I, C>(C instance) where C : I
        {
            _container.Instance<I, C>(instance);
        }
    }
}