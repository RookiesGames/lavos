using Godot;
using Lavos.Dependency;
using Lavos.Services.DeviceInfo;
using Lavos.Utils.Platform;

namespace Lavos.Services.DeviceInfo;

[GlobalClass]
public sealed partial class DeviceInfoConfig : Config
{
    const string Tag = nameof(DeviceInfoConfig);

    public override void Configure(IDependencyBinder binder)
    {
        switch (PlatformUtils.CurrentPlatform)
        {
            case PlatformUtils.Platform.Android:
                binder.Bind<IDeviceInfo, AndroidDeviceInfo>();
                return;
            default:
                binder.Bind<IDeviceInfo, DummyDeviceInfo>();
                return;
        }
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        resolver.Resolve<IDeviceInfo>();
    }
}