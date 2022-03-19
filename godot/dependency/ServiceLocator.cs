using System.Collections.Generic;
using Godot;
using Lavos.Debug;

namespace Lavos.Dependency
{
    public sealed class ServiceLocator : Node
    {
        private static DependencyContainer _container = null;


        public ServiceLocator(DependencyContainer container) 
        {
            Assert.IsTrue(_container == null, "Service Locator already built!");
            _container = container;
        }

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
    }
}
