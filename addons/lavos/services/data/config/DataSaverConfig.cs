using Godot;
using Lavos.Dependency;

namespace Lavos.Services.Data;

public partial class DataSaverConfig : Config
{
    [Export] public DataSaver[] _dataSavers;

    public override void Initialize(IDependencyResolver resolver)
    {
        var service = resolver.Resolve<IDataSaverService>();
        foreach (var saver in _dataSavers)
        {
            service.Register(saver);
            service.ReadData(saver);
        }
    }
}
