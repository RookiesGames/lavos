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
    readonly Dictionary<string, Type> bindings = [];
    readonly Dictionary<string, Type> lookups = [];
    readonly Dictionary<string, IService> instances = [];

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
        Assert.IsFalse(bindings.ContainsKey(typeof(I).FullName), "Binding cannot be duplicated");
        //
        DoBind<I, C>();
    }

    void DoBind<I, C>()
    {
        bindings[typeof(I).FullName] = typeof(C);
    }

    public void Lookup<I1, I2>() where I2 : I1, IService
    {
        Assert.IsTrue(typeof(I1).IsInterface, "Only interfaces can be lookup from");
        Assert.IsTrue(typeof(I2).IsInterface, "Only interfaces can be lookup to");
        Assert.IsTrue(typeof(I2).IsAssignableTo(typeof(I1)), $"Type mismatch - {typeof(I2)} does not inherit from {typeof(I1)}");
        lookups[typeof(I1).FullName] = typeof(I2);
    }

    public void Instance<I, C>(C instance) where I : IService where C : I
    {
        DoBind<I, C>();
        AddInstance(typeof(C), instance);
    }

    public void Instance<C>(C instance)
    {
        DoBind<C, C>();
        AddInstance(typeof(C), (IService)instance);
    }

    public T Resolve<T>()
    {
        return (T)FindOrCreateType(typeof(T));
    }

    public IService FindOrCreateType(Type type)
    {
        if (bindings.TryGetValue(type.FullName, out Type value))
        {
            return GetOrCreateInstance(value);
        }
        //
        return LookUpType(type);
    }

    IService GetOrCreateInstance(Type type)
    {
        var key = type.FullName;
        if (instances.DoesNotContainKey(key))
        {
            var service = CreateInstance(type);
            if (service is Node node)
            {
                node.Name = type.Name;
                _nodes.AddChild(node);
            }
            //
            AddInstance(type, service);
            InjectDependencies(service);
        }
        return instances[key];
    }

    static IService CreateInstance(Type type)
    {
        return (IService)Activator.CreateInstance(type);
    }

    void AddInstance(Type type, IService obj)
    {
        instances[type.FullName] = obj;
    }

    void InjectDependencies(IService obj)
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

    void InjectProperty(PropertyInfo info, IService target)
    {
        var value = FindOrCreateType(info.PropertyType);
        info.SetValue(target, value);
    }

    void InjectField(FieldInfo info, IService target)
    {
        var value = FindOrCreateType(info.FieldType);
        info.SetValue(target, value);
    }

    IService LookUpType(Type type)
    {
        Assert.IsTrue(type.IsInterface, "Only interfaces can be looked up for");
        //
        if (lookups.TryGetValue(type.FullName, out Type value))
        {
            return FindOrCreateType(value) ?? LookUpType(value);
        }
        //
        Assert.Fail($"Failed to lookup type {type}");
        return null;
    }
}