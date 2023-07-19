using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lavos.Dependency;

public sealed partial class DependencyContainer
    : Node
    , IDependencyBinder
    , IDependencyResolver
{
    Node _nodes;
    readonly Dictionary<Type, Type> bindings = new();
    readonly Dictionary<Type, List<Type>> lookups = new();
    readonly Dictionary<Type, object> instances = new();

    public override void _Ready()
    {
        _nodes = this.AddNode<Node>("Nodes");
    }

    public override void _ExitTree()
    {
        bindings.Clear();
        lookups.Clear();
        instances.Clear();
    }

    public void Bind<I, C>() where I : IService where C : I, new()
    {
        Assert.IsTrue(typeof(I).IsInterface, "Only interfaces can be bound from");
        Assert.IsTrue(typeof(C).IsClass, "Only classes can be bound to");
        Assert.IsTrue(typeof(C).IsAssignableTo(typeof(I)), $"Type mismatch - {typeof(C)} does not inherit from {typeof(I)}");
        Assert.IsFalse(bindings.ContainsKey(typeof(I)), "Binding cannot be duplicated");
        //
        DoBind<I, C>();
    }

    void DoBind<I, C>()
    {
        bindings[typeof(I)] = typeof(C);
    }

    public void Lookup<I1, I2>() where I2 : I1, IService
    {
        Assert.IsTrue(typeof(I1).IsInterface, "Only interfaces can be lookup from");
        Assert.IsTrue(typeof(I2).IsInterface, "Only interfaces can be lookup to");
        Assert.IsTrue(typeof(I2).IsAssignableTo(typeof(I1)), $"Type mismatch - {typeof(I2)} does not inherit from {typeof(I1)}");
        //
        if (lookups.DoesNotContainKey(typeof(I1)))
        {
            lookups[typeof(I1)] = new List<System.Type>();
        }
        lookups[typeof(I1)].Add(typeof(I2));
    }

    public void Instance<I, C>(C instance) where I : IService where C : I
    {
        DoBind<I, C>();
        AddInstance(typeof(C), (object)instance);
    }

    public void Instance<C>(C instance)
    {
        DoBind<C, C>();
        AddInstance(typeof(C), (object)instance);
    }

    public T Resolve<T>()
    {
        return (T)FindOrCreateType(typeof(T));
    }

    public object FindOrCreateType(Type type)
    {
        if (bindings.ContainsKey(type))
        {
            var realType = bindings[type];
            return GetOrCreateInstance(realType);
        }
        //
        return LookUpType(type);
    }

    object GetOrCreateInstance(Type type)
    {
        if (instances.DoesNotContainKey(type))
        {
            var obj = CreateInstance(type);
            if (obj is Node node)
            {
                node.Name = type.Name;
                _nodes.AddChild(node);
            }
            //
            AddInstance(type, obj);
            InjectDependencies(obj);
        }
        return instances[type];
    }

    static object CreateInstance(Type type)
    {
        return Activator.CreateInstance(type);
    }

    void AddInstance(Type type, object obj)
    {
        instances[type] = obj;
    }

    void InjectDependencies(object obj)
    {
        var realType = obj.GetType();

        var flags = BindingFlags.Public | BindingFlags.NonPublic;
        flags |= BindingFlags.DeclaredOnly | BindingFlags.Instance;
        flags |= BindingFlags.SetProperty;

        foreach (var field in realType.GetFields(flags))
        {
            if (field.HasCustomAttribute<InjectAttribute>())
            {
                InjectField(field, obj);
            }
        }

        foreach (var property in realType.GetProperties(flags))
        {
            if (property.HasCustomAttribute<InjectAttribute>())
            {
                InjectProperty(property, obj);
            }
        }

        foreach (var method in realType.GetMethods(flags))
        {
            if (method.HasCustomAttribute<InjectMethodAttribute>())
            {
                method.Invoke(obj, null);
            }
        }
    }

    void InjectProperty(PropertyInfo info, object target)
    {
        var value = FindOrCreateType(info.PropertyType);
        info.SetValue(target, value);
    }

    void InjectField(FieldInfo info, object target)
    {
        var value = FindOrCreateType(info.FieldType);
        info.SetValue(target, value);
    }

    object LookUpType(Type type)
    {
        Assert.IsTrue(type.IsInterface, "Only interfaces can be looked up for");
        //
        if (lookups.ContainsKey(type))
        {
            var list = lookups[type];
            Assert.IsTrue(list.Count == 1, "Type has more than one lookup. Consider looking up as a list");
            //
            var lookupType = list[0];
            return FindOrCreateType(lookupType) ?? LookUpType(lookupType);
        }
        //
        Assert.Fail($"Failed to lookup type {type}");
        return null;
    }

    internal List<object> FindList(Type type)
    {
        Assert.IsTrue(type.IsInterface, "Only interfaces can be searched for");
        return LookUpList(type);
    }

    List<object> LookUpList(Type type)
    {
        Assert.IsTrue(type.IsInterface, "Only interfaces can be looked up for");
        //
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
        //
        return null;
    }
}