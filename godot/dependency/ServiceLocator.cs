using System.Collections.Generic;
using Godot;

namespace Lavos.Dependency;

public sealed partial class ServiceLocator : Node
{
    static DependencyContainer _container = null;


    public ServiceLocator(DependencyContainer container)
    {
        Assert.IsTrue(_container == null, "Service Locator already built!");
        _container = container;
    }

    public static T Locate<T>() where T : IService
    {
        var type = typeof(T);
        var obj = (T)_container.FindOrCreateType(type);
        Assert.IsTrue(obj != null, $"Could not locate type {typeof(T)}");
        return obj;
    }

    public static List<T> LocateAsList<T>() where T : IService
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

    public static void Register<I, C>(C instance) where C : I, IService
    {
        _container.Instance<I, C>(instance);
    }

    public static void Register<C>(C instance) where C : IService
    {
        _container.Instance<C>(instance);
    }
}