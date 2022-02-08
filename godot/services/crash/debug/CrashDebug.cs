using Godot;
using Lavos.Core.Dependency;
using Lavos.Utils.Lazy;
using System;
using System.Threading.Tasks;

namespace Lavos.Services.Crash.Debug
{
    sealed class CrashDebug : Node
    {
        ICrashService _crashService;

        public override void _Ready()
        {
            _crashService = ServiceLocator.Locate<ICrashService>();
        }

        public void OnLogEvent()
        {
            _crashService.Log("Log");
        }

        public void OnLogException()
        {
            _crashService.LogException(new NullReferenceException("Exception"));
        }

        public void OnTaskLog()
        {
            Task.Run(() => { _crashService.Log("Task"); });
        }

        public void OnTaskException()
        {
            Task.Run(() => { _crashService.LogException(new TaskCanceledException("Exception")); });
        }
    }
}