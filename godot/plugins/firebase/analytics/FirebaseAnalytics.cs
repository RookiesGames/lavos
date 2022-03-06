using Godot;
using Godot.Collections;
using Lavos.Debug;
using Lavos.Services.Analytics;

namespace Lavos.Plugins.Firebase.Analytics
{
    sealed class FirebaseAnalytics : IAnalyticsService
    {
        const string PluginName = "FirebaseAnalytics";
        readonly LavosPlugin _plugin;

        public FirebaseAnalytics()
        {
            Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugins {PluginName}");
            _plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
        }

        public void Initialise()
        {
            _plugin.CallVoid("init");
        }

        public void EnableCollection(bool enable)
        {
            _plugin.CallVoid("setAnalyticsCollectionEnabled", enable);
        }

        public void SetDefaultParameters(Dictionary<string, object> parameters)
        {
            _plugin.CallVoid("setDefaultEventParameters", parameters);
        }

        public void LogEvent(string name)
        {
            _plugin.CallVoid("logEvent");
        }

        public void LogEvent(string name, Dictionary<string, object> parameters)
        {
            _plugin.CallVoid("logEvent", name, parameters);
        }

        public void ResetData()
        {
            _plugin.CallVoid("resetAnalyticsData");
        }

        public void SetUserId(string id)
        {
            _plugin.CallVoid("setUserId", id);
        }

        public void SetUserProperty(string name, string value)
        {
            _plugin.CallVoid("setUserProperty", name, value);
        }
    }
}