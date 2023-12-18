using Godot;

namespace Lavos.Services.DeviceInfo;

sealed class AndroidDeviceInfo : IDeviceInfo
{
    const string PluginName = "DeviceInfo";
    readonly LavosPlugin Plugin;

    public AndroidDeviceInfo()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public string Platform => Plugin.CallString("getPlatform");
    public string OSVersion => Plugin.CallString("getOSVersion");
    public string DeviceName => Plugin.CallString("getDeviceName");
    public string VersionName => Plugin.CallString("getVersionName");
    public string VersionCode => Plugin.CallString("getVersionCode");
}
