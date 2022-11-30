using Lavos.Dependency;

namespace Lavos.Services.Data
{
    public sealed partial class DataSaverConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<IDataSaverService, DataSaverService>();
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            resolver.Resolve<IDataSaverService>();
        }
    }
}