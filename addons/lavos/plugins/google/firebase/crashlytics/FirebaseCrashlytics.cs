using Godot;
using Lavos.Services.Crash;
using System;

namespace Lavos.Plugins.Google.Firebase.Crashlytics;

sealed class FirebaseCrashlytics : ICrashService
{
    const string PluginName = "FirebaseCrashlytics";
    readonly LavosPlugin Plugin;

    public FirebaseCrashlytics()
    {
        Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugin {PluginName}");
        Plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
    }

    public void Initialise()
    {
        Plugin.CallVoid("init");
    }

    #region ICrashService

    public void EnableCollection(bool enable)
    {
        Plugin.CallVoid("setCrashlyticsCollectionEnabled", enable);
    }

    public bool CheckForUnsentReports()
    {
        return Plugin.CallBool("checkForUnsentReports");
    }

    public void SendUnsentReports()
    {
        Plugin.CallVoid("sendUnsentReports");
    }

    public void DeleteUnsentReports()
    {
        Plugin.CallVoid("deleteUnsentReports");
    }

    public bool DidCrashOnPreviousExecution()
    {
        return Plugin.CallBool("didCrashOnPreviousExecution");
    }

    public void Log(string message)
    {
        Plugin.CallVoid("log", message);
    }

    public void LogException(Exception e)
    {
        var exception = $"{e.GetType()} | {e.Message}";
        Plugin.CallVoid("recordException", exception);
    }

    public void SetUserId(string id)
    {
        Plugin.CallVoid("setUserId", id);
    }

    public void SetCustomKey(string key, Variant value)
    {
        var method = string.Empty;
        switch (value.VariantType)
        {
            case Variant.Type.Int: Plugin.CallVoid("setCustomKeyI", key, value); return;
            case Variant.Type.Float: Plugin.CallVoid("setCustomKeyF", key, value); return;
            case Variant.Type.Bool: Plugin.CallVoid("setCustomKeyB", key, value); return;
            case Variant.Type.String: Plugin.CallVoid("setCustomKeyS", key, value); return;
            default:
                {
                    Plugin.CallVoid("setCustomKeyS", key, "TypeNotSupported");
                    Console.Log.Error(nameof(FirebaseCrashlytics), "Unhandled custom type");
                    return;
                }
        }
    }

    public void NativeCrash()
    {
#if DEBUG
        Plugin.CallVoid("crash");
#endif
    }

    #endregion ICrashService
}