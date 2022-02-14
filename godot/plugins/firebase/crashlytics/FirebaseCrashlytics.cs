using System;
using Godot;
using Lavos.Core.Debug;
using Lavos.Services.Crash;

namespace Lavos.Plugins.Firebase.Crashlytics
{
    sealed class FirebaseCrashlytics : ICrashService
    {
        const string PluginName = "FirebaseCrashlytics";
        readonly LavosPlugin _plugin;

        public FirebaseCrashlytics()
        {
            Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugins {PluginName}");
            _plugin = new LavosPlugin(Engine.GetSingleton(PluginName));
        }

        public void Initialise()
        {
            _plugin.CallVoid("init");
        }

        #region ICrashService

        public void EnableCollection(bool enable)
        {
            _plugin.CallVoid("setCrashlyticsCollectionEnabled", enable);
        }

        public bool CheckForUnsentReports()
        {
            return _plugin.Call<bool>("checkForUnsentReports");
        }

        public void SendUnsentReports()
        {
            _plugin.CallVoid("sendUnsentReports");
        }

        public void DeleteUnsentReports()
        {
            _plugin.CallVoid("deleteUnsentReports");
        }

        public bool DidCrashOnPreviousExecution()
        {
            return _plugin.Call<bool>("didCrashOnPreviousExecution");
        }

        public void Log(string message)
        {
            _plugin.CallVoid("log", message);
        }

        public void LogException(Exception e)
        {
            var exception = $"{e.GetType()} | {e.Message}";
            _plugin.CallVoid("recordException", exception);
        }

        public void SetUserId(string id)
        {
            _plugin.CallVoid("setUserId", id);
        }

        public void SetCustomKey<T>(string key, T value)
        {
            var method = string.Empty;
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Int16:
                case TypeCode.Int32: _plugin.CallVoid("setCustomKeyI", key, value); return;
                case TypeCode.Single: _plugin.CallVoid("setCustomKeyF", key, value); return;
                case TypeCode.Boolean: _plugin.CallVoid("setCustomKeyB", key, value); return;
                case TypeCode.String: _plugin.CallVoid("setCustomKeyS", key, value); return;
                default:
                    {
                        _plugin.CallVoid("setCustomKeyS", key, "TypeNotSupported");
                        Core.Console.Log.Error(nameof(FirebaseCrashlytics), "Unhandled custom type");
                        return;
                    }
            }
        }

        public void NativeCrash()
        {
#if DEBUG
            _plugin.CallVoid("crash");
#endif
        }

        #endregion ICrashService
    }
}
