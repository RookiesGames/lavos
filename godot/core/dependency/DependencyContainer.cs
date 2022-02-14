using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using Lavos.Core.Debug;
using Lavos.Utils.Extensions;
using Lavos.Core.Console;

namespace Lavos.Core.Dependency
{
    public sealed class DependencyContainer
        : Node
        , IDependencyBinder
        , IDependencyResolver
    {
        #region Members

        private static DependencyContainer _singleton;
        private readonly Dictionary<System.Type, System.Type> bindings = new Dictionary<System.Type, System.Type>();
        private readonly Dictionary<System.Type, List<System.Type>> lookups = new Dictionary<System.Type, List<System.Type>>();
        private readonly Dictionary<System.Type, object> instances = new Dictionary<System.Type, object>();

        #endregion


        #region Properties

        public static DependencyContainer Singleton => _singleton;

        #endregion


        #region Contructor

        internal DependencyContainer()
        {
            _singleton = this;
            Log.Debug(nameof(DependencyContainer), "Node built");
        }

        #endregion Constructor


        #region IDependencyContainer

        public void Bind<I, C>() where C : I, new()
        {
            Assert.IsTrue(typeof(I).IsInterface, "Only interfaces can be bound from");
            Assert.IsTrue(typeof(C).IsClass, "Only classes can be bound to");
            Assert.IsTrue(typeof(C).IsAssignableTo(typeof(I)), $"Type mismatch - {typeof(C)} does not inherit from {typeof(I)}");
            Assert.IsFalse(bindings.ContainsKey(typeof(I)), "Binding cannot be duplicated");

            DoBind<I, C>();
        }

        public void DoBind<I, C>()
        {
            bindings[typeof(I)] = typeof(C);
        }

        public void Lookup<I1, I2>() where I2 : I1
        {
            Assert.IsTrue(typeof(I1).IsInterface, "Only interfaces can be lookup from");
            Assert.IsTrue(typeof(I2).IsInterface, "Only interfaces can be lookup to");
            Assert.IsTrue(typeof(I2).IsAssignableTo(typeof(I1)), $"Type mismatch - {typeof(I2)} does not inherit from {typeof(I1)}");

            if (lookups.DoesNotContainKey(typeof(I1)))
            {
                lookups[typeof(I1)] = new List<System.Type>();
            }
            lookups[typeof(I1)].Add(typeof(I2));
        }

        public void Instance<I, C>(C instance) where C : I
        {
            DoBind<I, C>();
            AddInstance(typeof(C), (object)instance);
        }

        #endregion IDependencyContainer


        #region DependencyResolver

        public T Resolve<T>()
        {
            return (T)FindOrCreateType(typeof(T));
        }

        #endregion DependencyResolver


        #region Single

        public object FindOrCreateType(Type type)
        {
            Assert.IsTrue(type.IsInterface, "Only interfaces can be searched for");

            if (bindings.ContainsKey(type))
            {
                var realType = bindings[type];
                return GetOrCreateInstance(realType);
            }

            return LookUpType(type);
        }

        private object GetOrCreateInstance(Type type)
        {
            if (instances.DoesNotContainKey(type))
            {
                var obj = CreateInstance(type);
                if (obj is Node)
                {

                }
                AddInstance(type, obj);
                InjectDependencies(obj);
            }
            return instances[type];
        }

        private object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private void AddInstance(Type type, object obj)
        {
            instances[type] = obj;
        }

        private void InjectDependencies(object obj)
        {
            var realType = obj.GetType();

            var flags = BindingFlags.Public | BindingFlags.NonPublic;
            flags = flags | BindingFlags.DeclaredOnly | BindingFlags.Instance;
            flags = flags | BindingFlags.SetProperty;

            var fieldInfos = realType.GetFields(flags);
            foreach (var field in fieldInfos)
            {
                if (field.HasCustomAttribute<InjectAttribute>())
                {
                    InjectField(field, obj);
                }
            }

            var propertyInfos = realType.GetProperties(flags);
            foreach (var property in propertyInfos)
            {
                if (property.HasCustomAttribute<InjectAttribute>())
                {
                    InjectProperty(property, obj);
                }
            }

            var methods = realType.GetMethods(flags);
            foreach (var method in methods)
            {
                if (method.HasCustomAttribute<InjectMethodAttribute>())
                {
                    method.Invoke(obj, null);
                }
            }

            return;
        }

        private void InjectProperty(PropertyInfo info, object target)
        {
            var value = FindOrCreateType(info.PropertyType);
            info.SetValue(target, value);
        }

        private void InjectField(FieldInfo info, object target)
        {
            var value = FindOrCreateType(info.FieldType);
            info.SetValue(target, value);
        }

        private object LookUpType(Type type)
        {
            Assert.IsTrue(type.IsInterface, "Only interfaces can be looked up for");

            if (lookups.ContainsKey(type))
            {
                var list = lookups[type];
                Assert.IsTrue(list.Count == 1, "Type has more than one lookup. Consider looking up as a list");

                var lookupType = list[0];
                return FindOrCreateType(lookupType) ?? LookUpType(lookupType);
            }

            Assert.Fail($"Failed to lookup type {type}");
            return null;
        }

        #endregion


        #region List

        internal List<object> FindList(Type type)
        {
            Assert.IsTrue(type.IsInterface, "Only interfaces can be searched for");
            return LookUpList(type);
        }

        private List<object> LookUpList(Type type)
        {
            Assert.IsTrue(type.IsInterface, "Only interfaces can be looked up for");

            if (lookups.ContainsKey(type))
            {
                var list = lookups[type];
                var lookupList = new List<object>();
                foreach (var t in list)
                {
                    lookupList.Add(FindOrCreateType(t));
                }
                return lookupList;
            }

            return null;
        }

        #endregion
    }
}