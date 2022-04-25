using Lavos.Dependency;
using Lavos.Audio;
using Lavos.Utils.Threading;

namespace Lavos.Boot
{
    public sealed class BaseConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<IThreadDispatcher, MainThreadDispatcher>();
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            resolver.Resolve<IThreadDispatcher>();
        }
    }
}