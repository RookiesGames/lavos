using Lavos.Core;

namespace Lavos.Dependency;

public static class ServiceLocator
{
    static LazyNode<DependencyContainer> _dependencyContainer = new();

    public static T Locate<T>() where T : IService
    {
        var type = typeof(T);
        var obj = (T)_dependencyContainer.Node.FindOrCreateType(type);
        Assert.IsTrue(obj != null, $"Could not locate type {typeof(T)}");
        return obj;
    }

    public static void Register<I, C>(C instance) where I : IService where C : I
    {
        _dependencyContainer.Node.Instance<I, C>(instance);
    }

    public static void Register<C>(C instance) where C : IService
    {
        _dependencyContainer.Node.Instance<C>(instance);
    }
}