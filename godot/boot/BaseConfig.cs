using Lavos.Core.Dependency;
using Lavos.Audio;

namespace Lavos.Boot
{
    public class BaseConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<ISoundManager, SoundManager>();
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            resolver.Resolve<ISoundManager>();
        }
    }
}