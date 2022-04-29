using System;

namespace Lavos.Services.Crash
{
    public interface ICrashService
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
        void SetCustomKey<T>(string key, T value);

        void NativeCrash();
    }
}