using Lavos.Core.Debug;
using Lavos.Core.Dependency;
using Lavos.Plugins.Firebase.Crashlytics;
using Lavos.Services.Crash;
using Lavos.Utils.Platform;

namespace Lavos.Services.Analytics
{
    public sealed class CrashConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            if (PlatformUtils.IsAndroid || PlatformUtils.IsiOS)
            {
                binder.Bind<ICrashService, FirebaseCrashlytics>();
            }
            else
            {
                binder.Bind<ICrashService, DummyCrashService>();
            }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            var resolved = resolver.Resolve<ICrashService>();
            Assert.IsTrue(resolved, $"Type {nameof(ICrashService)} was not resolved");
        }
    }
}