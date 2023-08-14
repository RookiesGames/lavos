
using System;

namespace Lavos.Services.Crash;

sealed class DummyCrashService : ICrashService
{
    public void Initialize() { }

    public void EnableCollection(bool enable) { }

    public bool CheckForUnsentReports() => false;
    public void SendUnsentReports() { }
    public void DeleteUnsentReports() { }

    public bool DidCrashOnPreviousExecution() => false;

    public void Log(string message) { }
    public void LogException(Exception e) { }

    public void SetUserId(string id) { }
    public void SetCustomKey(string key, Godot.Variant value) { }

    public void NativeCrash() { }
}
