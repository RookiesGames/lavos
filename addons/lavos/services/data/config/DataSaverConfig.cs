using Godot;
using Godot.Collections;
using Lavos.Dependency;

namespace Lavos.Services.Data;

public sealed partial class DataSaverConfig : Config
{
    [Export] DataSaver[] _dataSavers;

    public override void Configure(IDependencyBinder binder)
    {
        binder.Bind<IDataSaverService, DataSaverService>();
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<IDataSaverService>();
        foreach (var saver in _dataSavers)
        {
            service.Register(saver);
            service.Load(saver);
        }
    }
}
