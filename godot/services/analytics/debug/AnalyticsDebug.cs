using Godot;
using Godot.Collections;
using Lavos.Dependency;
using System.Threading.Tasks;

namespace Lavos.Services.Analytics.Debug
{
    sealed class AnalyticsDebug : Node
    {
        IAnalyticsService _service;

        public override void _Ready()
        {
            _service = ServiceLocator.Locate<IAnalyticsService>();
            _service.EnableCollection(true);
            //
            _service.SetUserId("user_analytics123");
            //
            var parameters = new Dictionary<string, object>();
            parameters.Add("version", "1.0");
            parameters.Add("os", "Android 42");
            parameters.Add("level", 24);
            _service.SetDefaultParameters(parameters);
            //
            _service.SetUserProperty("race", "caucasian");
        }

        public void OnLogEvent()
        {
            Lavos.Console.Log.Debug("fsd", "fsdaf");
            _service.LogEvent("debug");
            _service.LogEvent("debug params", new Dictionary<string, object>() { { "key1", "value1" }, { "key2", "value2" } });
        }

        public void OnTaskLogEvent()
        {
            Task.Run(() =>
            {
                _service.LogEvent("task");
                _service.LogEvent("task params", new Dictionary<string, object>() { { "key1", "value1" }, { "key2", "value2" } });
            });
        }
    }
}