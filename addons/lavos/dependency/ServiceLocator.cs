using System.Collections.Generic;
using Godot;

namespace Lavos.Dependency;

public sealed partial class ServiceLocator : Node
{
    static DependencyContainer _container;

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

    public static void Register<I, C>(C instance) where I : IService where C : I
    {
        _container.Instance<I, C>(instance);
    }

    public static void Register<C>(C instance) where C : IService
    {
        _container.Instance<C>(instance);
    }
}