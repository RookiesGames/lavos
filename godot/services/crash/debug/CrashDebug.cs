using Godot;
using Lavos.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Services.Crash.Debug
{
    sealed partial class CrashDebug : Node
    {
        ICrashService _crashService;

        public override void _Ready()
        {
            _crashService = ServiceLocator.Locate<ICrashService>();
            _crashService.EnableCollection(true);
            //
            _crashService.SetUserId("user_crash1234");
            _crashService.SetCustomKey("int", Godot.Variant.CreateFrom(123));
            _crashService.SetCustomKey("long", Godot.Variant.CreateFrom(123));
            _crashService.SetCustomKey("float", Godot.Variant.CreateFrom(123.123f));
            _crashService.SetCustomKey("double", Godot.Variant.CreateFrom(123.123));
            _crashService.SetCustomKey("bool", Godot.Variant.CreateFrom(true));
            _crashService.SetCustomKey("string", Godot.Variant.CreateFrom("abc"));
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