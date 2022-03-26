using Lavos.Dependency;
using Lavos.Audio;
using Lavos.Utils.Threading;

namespace Lavos.Boot
{
    public class BaseConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<IMusicManager, MusicManager>();
            binder.Bind<ISoundManager, SoundManager>();
            binder.Bind<IThreadDispatcher, MainThreadDispatcher>();
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            resolver.Resolve<IMusicManager>();
            resolver.Resolve<ISoundManager>();
            resolver.Resolve<IThreadDispatcher>();
        }
    }
}