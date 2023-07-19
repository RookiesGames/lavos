using Lavos.Dependency;
using System;

namespace Lavos.Services.Crash;

public interface ICrashService : IService
{
    void Initialise();

    void EnableCollection(bool enable);

    bool CheckForUnsentReports();
    void SendUnsentReports();
    void DeleteUnsentReports();

    bool DidCrashOnPreviousExecution();

    void Log(string message);
    void LogException(Exception e);

    void SetUserId(string id);
    void SetCustomKey(string key, Godot.Variant value);

    void NativeCrash();
}
