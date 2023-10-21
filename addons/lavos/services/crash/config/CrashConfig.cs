using Godot;
using Lavos.Dependency;
using Lavos.Services.Crash.Firebase;
using Lavos.Utils.Platform;

namespace Lavos.Services.Crash;

[GlobalClass]
public sealed partial class CrashConfig : Config
{
    const string Tag = nameof(CrashConfig);

    public enum Provider
    {
        None,
        Firebase,
    }

    [Export] Provider crashProvider;

    public override void Configure(IDependencyBinder binder)
    {
        if (!PlatformUtils.IsMobile)
        {
            crashProvider = Provider.None;
        }
        //
        switch (crashProvider)
        {
            case Provider.Firebase:
                {
                    if (FirebaseCrashlyticsService.IsPluginEnabled())
                    {
                        Log.Info(Tag, "Firebase Crashlytics plugin enabled");
                        binder.Bind<ICrashService, FirebaseCrashlyticsService>();
                    }
                    else
                    {
                        Log.Warn(Tag, "Firebase Crashlytics plugin disabled");
                        goto default;
                    }
                    break;
                }
            default:
                {
                    Log.Info(Tag, "No binding provided");
                    binder.Bind<ICrashService, DummyCrashService>();
                    break;
                }
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<ICrashService>();
        service.Initialize();
    }
}
