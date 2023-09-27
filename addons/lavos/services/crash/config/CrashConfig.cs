using Godot;
using Lavos.Dependency;
using Lavos.Plugins.Google.Firebase.Crashlytics;
using Lavos.Utils.Platform;

namespace Lavos.Services.Crash;

[GlobalClass]
public sealed partial class CrashConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        if (PlatformUtils.IsMobile)
        {
            if (FirebaseCrashlytics.IsPluginEnabled())
            {
                Log.Info(nameof(CrashConfig), "Firebase Crashlytics plugin enabled");
                binder.Bind<ICrashService, FirebaseCrashlytics>();
            }
            else
            {
                Log.Warn(nameof(CrashConfig), "Firebase Crashlytics plugin disabled");
                binder.Bind<ICrashService, DummyCrashService>();
            }
        }
        else
        {
            binder.Bind<ICrashService, DummyCrashService>();
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<ICrashService>();
        Assert.IsTrue(service != null, $"Type {nameof(ICrashService)} was not resolved");
        service.Initialize();
    }
}