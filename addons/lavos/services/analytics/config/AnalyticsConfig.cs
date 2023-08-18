using Godot;
using Lavos.Dependency;
using Lavos.Plugins.Google.Firebase.Analytics;
using Lavos.Utils.Platform;

namespace Lavos.Services.Analytics;

[GlobalClass]
public partial class AnalyticsConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        if (PlatformUtils.IsMobile)
        {
            if (FirebaseAnalytics.IsPluginEnabled())
            {
                binder.Bind<IAnalyticsService, FirebaseAnalytics>();
            }
            else
            {
                Log.Warn(nameof(AnalyticsConfig), "Firebase Analytics plugin not enabled");
                binder.Bind<IAnalyticsService, DummyAnalyticsService>();
            }
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
        service.Initialize();
    }
}
