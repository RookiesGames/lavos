using System;
using System.Text;
using Godot;
using Lavos.Core.Debug;
using Lavos.Core.Console;
using Lavos.Services.Crash;

namespace Lavos.Plugins.Firebase.Crashlytics
{
    sealed class FirebaseCrashlytics : ICrashService
    {
        const string PluginName = "FirebaseCrashlytics";
        readonly LavosPlugin _plugin;
        readonly StringBuilder _exceptionBuilder;

        FirebaseCrashlytics()
        {
            Assert.IsTrue(Engine.HasSingleton(PluginName), $"Missing plugins {PluginName}");
            _plugin = (LavosPlugin)Engine.GetSingleton(PluginName);
            _exceptionBuilder = new StringBuilder();
        }

        #region ICrashService

        public void EnableCollection(bool enable)
        {
            _plugin.CallVoid("setCrashlyticsCollectionEnabled", enable);
        }

        public bool CheckForUnsentReports()
        {
            return _plugin.Call<bool>("checkForUnsentReports", null);
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
            var exception = _exceptionBuilder
                    .Append(e.GetType())
                    .Append(" | ")
                    .Append(e.Message);
            _plugin.CallVoid("recordException", exception);
            _exceptionBuilder.Clear();
        }

        public void SetUserId(string id)
        {
            _plugin.CallVoid("setUserId", id);
        }

        public void SetCustomKey<T>(string key, T value)
        {
            var method = "setCustomKey";
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Boolean: _plugin.CallVoid(method, key, Convert.ToBoolean(value)); return;
                case TypeCode.Double: _plugin.CallVoid(method, key, Convert.ToDouble(value)); return;
                case TypeCode.Int64: _plugin.CallVoid(method, key, Convert.ToInt64(value)); return;
                case TypeCode.String: _plugin.CallVoid(method, key, Convert.ToString(value)); return;
                default: Core.Console.Log.Error(nameof(FirebaseCrashlytics), "Unhandled custom type"); return;
            }
        }

        #endregion ICrashService
    }
}
