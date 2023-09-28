using Godot;
using Godot.Collections;
using Lavos.Addons;
using Lavos.Services.Analytics;

namespace Lavos.Serivces.Analytics.Firebase;

sealed class FirebaseAnalytics : IAnalyticsService
{
    const string PluginName = nameof(FirebaseAnalytics);
    readonly LavosAddon Plugin;

    public FirebaseAnalytics()
    {
        Assert.IsTrue(IsPluginEnabled(), $"Missing plugin {PluginName}");
        Plugin = new LavosAddon(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");

    public void EnableCollection(bool enable) =>
        Plugin.CallVoid("setAnalyticsCollectionEnabled", enable);

    public void SetDefaultParameters(Dictionary<string, Variant> parameters) =>
        Plugin.CallVoid("setDefaultEventParameters", parameters);

    public void LogEvent(string name, Dictionary<string, Variant> parameters) =>
        Plugin.CallVoid("logEvent", name, parameters);

    public void ResetData() => Plugin.CallVoid("resetAnalyticsData");

    public void SetUserId(string id) => Plugin.CallVoid("setUserId", id);

    public void SetUserProperty(string name, string value) =>
        Plugin.CallVoid("setUserProperty", name, value);
}
