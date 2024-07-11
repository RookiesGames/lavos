using Lavos.Dependency;

namespace Lavos.Services.Data;

public sealed partial class DataSaverServiceConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        binder.Bind<IDataSaverService, DataSaverService>();
    }
}
