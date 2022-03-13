using System.Collections.Generic;
using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;
using Lavos.Console;

namespace Lavos.Dependency
{
    public sealed class ServiceLocator : Node
    {
        #region Members

        private static DependencyContainer _container;

        #endregion Members


        #region Node

        public override void _Ready()
        {
            _container = DependencyContainer.Singleton;
            Assert.IsTrue(_container != null, "Dependency container not found");
            //
            Log.Debug(nameof(ServiceLocator), "Node built");
        }

        #endregion Node


        #region Interface

        public static T Locate<T>()
        {
            var type = typeof(T);
            var obj = (T)_container.FindOrCreateType(type);
            Assert.IsTrue(obj != null, $"Could not locate type {typeof(T)}");
            return obj;
        }

        public static List<T> LocateAsList<T>()
        {
            var type = typeof(T);
            //
            var objs = _container.FindList(type);
            Assert.IsFalse(objs.Count == 0, $"Could not locate type {typeof(T)}");
            //
            var list = new List<T>(objs.Count);
            objs.ForEach(obj => list.Add((T)obj));
            //
            return list;
        }

        public static void Register<I, C>(C instance) where C : I
        {
            _container.Instance<I, C>(instance);
        }

        #endregion Interface
    }
}
