using Lavos.Dependency;

namespace Lavos.Services.Data
{
    public sealed class DataSaverConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            binder.Bind<IDataSaverService, DummyDataSaverService>();
        }

        public override void Initialize(IDependencyResolver resolver)
        { }
    }
}