using Lavos.Dependency;

namespace Lavos.Services.Data
{
    public sealed class DataSaverConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<IDataSaverService, CommonDataSaverService>();
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            resolver.Resolve<IDataSaverService>();
        }
    }
}