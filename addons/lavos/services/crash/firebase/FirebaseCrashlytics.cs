using Godot;
using Lavos.Addons;
using System;

namespace Lavos.Services.Crash.Firebase;

sealed class FirebaseCrashlytics : ICrashService
{
    const string PluginName = nameof(FirebaseCrashlytics);
    readonly LavosAddon Plugin;

    public FirebaseCrashlytics()
    {
        Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugin {PluginName}");
        Plugin = new LavosAddon(Engine.GetSingleton(PluginName));
    }

    public static bool IsPluginEnabled() => Engine.HasSingleton(PluginName);

    public void Initialize() => Plugin.CallVoid("init");
    public void EnableCollection(bool enable) => Plugin.CallVoid("setCrashlyticsCollectionEnabled", enable);
    public bool CheckForUnsentReports() => Plugin.CallBool("checkForUnsentReports");
    public void SendUnsentReports() => Plugin.CallVoid("sendUnsentReports");
    public void DeleteUnsentReports() => Plugin.CallVoid("deleteUnsentReports");
    public bool DidCrashOnPreviousExecution() => Plugin.CallBool("didCrashOnPreviousExecution");
    public void Log(string message) => Plugin.CallVoid("log", message);
    public void LogException(Exception e)
    {
        var exception = $"{e.GetType()} | {e.Message}";
        Plugin.CallVoid("recordException", exception);
    }
    public void SetUserId(string id) => Plugin.CallVoid("setUserId", id);
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
}
