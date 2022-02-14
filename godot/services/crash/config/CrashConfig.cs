using Lavos.Core.Debug;
using Lavos.Core.Dependency;
using Lavos.Plugins.Firebase.Crashlytics;
using Lavos.Utils.Platform;

namespace Lavos.Services.Crash
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
            var service = resolver.Resolve<ICrashService>();
            Assert.IsTrue(service != null, $"Type {nameof(ICrashService)} was not resolved");
            service.Initialise();
        }
    }
}