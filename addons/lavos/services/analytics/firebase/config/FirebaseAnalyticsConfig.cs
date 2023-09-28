using Godot;
using Lavos.Dependency;
using Lavos.Services.Analytics.Firebase;
using Lavos.Utils.Platform;

namespace Lavos.Services.Analytics.Firebase;

[GlobalClass]
public partial class AnalyticsConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        if (PlatformUtils.IsMobile)
        {
            if (FirebaseAnalytics.IsPluginEnabled())
            {
                Log.Info(nameof(AnalyticsConfig), "Firebase Analytics plugin enabled");
                binder.Bind<IAnalyticsService, FirebaseAnalytics>();
            }
            else
            {
                Log.Warn(nameof(AnalyticsConfig), "Firebase Analytics plugin disabled");
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
