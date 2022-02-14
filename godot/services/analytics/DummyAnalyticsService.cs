using Godot.Collections;

namespace Lavos.Services.Analytics
{
    sealed class DummyAnalyticsService : IAnalyticsService
    {
        public void Initialise() { }

        public void EnableCollection(bool enable) { }

        public void SetDefaultParameters(Dictionary<string, object> parameters) { }

        public void LogEvent(string name) { }
        public void LogEvent(string name, Dictionary<string, object> parameters) { }

        public void ResetData() { }

        public void SetUserId(string id) { }

        public void SetUserProperty(string name, string value) { }
    }
}
