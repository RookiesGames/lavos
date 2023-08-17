using Godot;
using Godot.Collections;
using Lavos.Services.Analytics;

namespace Lavos.Plugins.Google.Firebase.Analytics;

sealed class FirebaseAnalytics : IAnalyticsService
{
    const string PluginName = "FirebaseAnalytics";
    readonly LavosPlugin Plugin;

    public FirebaseAnalytics()
    {
        Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public void Initialize()
    {
        Plugin.CallVoid("init");
    }

    public void EnableCollection(bool enable)
    {
        Plugin.CallVoid("setAnalyticsCollectionEnabled", enable);
    }

    public void SetDefaultParameters(Dictionary<string, Godot.Variant> parameters)
    {
        Plugin.CallVoid("setDefaultEventParameters", parameters);
    }

    public void LogEvent(string name)
    {
        Plugin.CallVoid("logEvent");
    }

    public void LogEvent(string name, Dictionary<string, Godot.Variant> parameters)
    {
        Plugin.CallVoid("logEvent", name, parameters);
    }

    public void ResetData()
    {
        Plugin.CallVoid("resetAnalyticsData");
    }

    public void SetUserId(string id)
    {
        Plugin.CallVoid("setUserId", id);
    }

    public void SetUserProperty(string name, string value)
    {
        Plugin.CallVoid("setUserProperty", name, value);
    }
}
