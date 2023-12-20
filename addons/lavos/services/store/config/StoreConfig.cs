using Godot;
using Lavos.Dependency;
using Lavos.Services.GoogleBilling;
using Lavos.Utils.Platform;

namespace Lavos.Services.Store;

[GlobalClass]
public partial class StoreConfig : Config
{
    const string Tag = nameof(StoreConfig);

    public override void Configure(IDependencyBinder binder)
    {
        if (PlatformUtils.IsAndroid)
        {
            Log.Info(Tag, "GoogleBilling plugin enabled");
            binder.Bind<IStoreService, GoogleBillingStoreService>();
        }
        else
        {
            Log.Warn(Tag, "No binding provided");
            binder.Bind<IStoreService, DummyStoreService>();
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<IStoreService>();
        service.Initialize();
    }
}