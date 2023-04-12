using Lavos.Dependency;
using Lavos.Plugins.Firebase.Analytics;
using Lavos.Utils.Platform;

namespace Lavos.Services.Analytics;

public sealed partial class AnalyticsConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        if (PlatformUtils.IsAndroid || PlatformUtils.IsiOS)
        {
            binder.Bind<IAnalyticsService, FirebaseAnalytics>();
        }
        else
        {
            binder.Bind<IAnalyticsService, DummyAnalyticsService>();
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<IAnalyticsService>();
        Assert.IsTrue(service != null, $"Type {nameof(IAnalyticsService)} was not resolved");
        service.Initialise();
    }
}
