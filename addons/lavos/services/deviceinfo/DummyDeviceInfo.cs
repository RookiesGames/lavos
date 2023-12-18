
namespace Lavos.Services.DeviceInfo;

sealed class DummyDeviceInfo : IDeviceInfo
{
    public string Platform => "dummy platform";
    public string OSVersion => "dummy OS version";
    public string DeviceName => "dummy device name";
    public string VersionName => "dummy version name";
    public string VersionCode => "dummy version code";
}
