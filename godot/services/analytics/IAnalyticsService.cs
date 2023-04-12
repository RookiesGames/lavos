using Lavos.Dependency;
using Godot.Collections;

namespace Lavos.Services.Analytics;

public interface IAnalyticsService : IService
{
    void Initialise();

    void EnableCollection(bool enable);

    void SetDefaultParameters(Dictionary<string, Godot.Variant> parameters);

    void LogEvent(string name);
    void LogEvent(string name, Dictionary<string, Godot.Variant> parameters);
    void ResetData();

    void SetUserId(string id);
    void SetUserProperty(string name, string value);
}
