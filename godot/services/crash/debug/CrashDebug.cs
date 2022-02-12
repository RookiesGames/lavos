using Godot;
using Lavos.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Crash.Debug
{
    sealed class CrashDebug : Node
    {
        ICrashService _crashService;

        public override void _Ready()
        {
            _crashService = ServiceLocator.Locate<ICrashService>();
            _crashService.EnableCollection(true);
        }

        public void OnLogEvent()
        {
            _crashService.Log($"Log from {nameof(CrashDebug)}");
        }

        public void OnLogException()
        {
            _crashService.LogException(new KeyNotFoundException($"Exception from {nameof(CrashDebug)}"));
        }

        public void OnTaskLog()
        {
            Task.Run(() => { _crashService.Log("Log from Task"); });
        }

        public void OnTaskException()
        {
            Task.Run(() => { _crashService.LogException(new TaskCanceledException($"Task Exception from {nameof(CrashDebug)}")); });
        }

        public void OnNativeCrash()
        {
            _crashService.NativeCrash();
        }

        public void OnNullReference()
        {
            List<int> list = null;
            list.Add(0);
        }

        public void OnMemoryCrash()
        {
            var list = new List<byte[]>();
            while (true)
            {
                list.Add(new byte[1024 * 1024 * 1024]);
            }
        }
    }
}