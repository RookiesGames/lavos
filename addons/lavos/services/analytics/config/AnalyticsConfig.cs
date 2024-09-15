using Godot;
using Lavos.Dependency;
using Lavos.Services.Analytics.Firebase;
using Lavos.Utils.Platform;

namespace Lavos.Services.Analytics;

[GlobalClass]
public partial class AnalyticsConfig : Config
{
    const string Tag = nameof(AnalyticsConfig);

    public enum Provider
    {
        None,
        Firebase,
    }

    [Export] Provider analyticsProvider;

    public override void Configure(IDependencyBinder binder)
    {
        //
        switch (analyticsProvider)
        {
            case Provider.Firebase:
                {
                    if (!PlatformUtils.IsMobile)
                    {
                        Log.Warn(Tag, "Firebase provider not available");
                        binder.Bind<IAnalyticsService, DummyAnalyticsService>();
                    }
                    else if (FirebaseAnalyticsService.IsPluginEnabled())
                    {
                        Log.Info(Tag, "Firebase Analytics plugin enabled");
                        binder.Bind<IAnalyticsService, FirebaseAnalyticsService>();
                    }
                    else
                    {
                        Log.Warn(Tag, "Firebase Analytics plugin disabled");
                        goto default;
                    }
                    break;
                }
            default:
                {
                    Log.Info(Tag, "No binding provided");
                    binder.Bind<IAnalyticsService, DummyAnalyticsService>();
                    break;
                }
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<IAnalyticsService>();
        service.Initialize();
    }
}
