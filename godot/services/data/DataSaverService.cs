/*
using Godot;
using Lavos.Console;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lavos.Services.Data
{
    sealed partial class DataSaverService
        : Node
        , IDataSaverService
    {
        const string Tag = nameof(DataSaverService);
        const string SavePath = "user://saves";
        const float SaveTimer = 0.25f;

        double _timer = 0;
        List<IDataSaver> _dataSavers = new List<IDataSaver>();
        JsonSerializer _serializer = new JsonSerializer();


        #region IDataSaverService

        public void Register(IDataSaver saver)
        {
            _dataSavers.PushUnique(saver);
        }

        public void Unregister(IDataSaver saver)
        {
            _dataSavers.Remove(saver);
        }

        public void CleanData()
        {
            var dir = Godot.DirAccess.Open(SavePath);
            if (Godot.DirAccess.GetOpenError() != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open directory {SavePath}");
                return;
            }

            Godot.DirAccess.RemoveAbsolute(dir.GetCurrentDir());
        }

        public void Load()
        {
            foreach (var dataSaver in _dataSavers)
            {
                Load(dataSaver);
            }
        }

        public void Load(IDataSaver saver)
        {
            var path = $"{SavePath}/{saver.DataFile}";
            using var file = (Godot.FileAccess.FileExists(path))
                                ? Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read)
                                : Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.WriteRead);
            //
            if (Godot.FileAccess.GetOpenError() != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }
            //
            var content = file.GetAsText();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            saver.LoadData(data);
        }

        public void Save()
        {
            foreach (var dataSaver in _dataSavers)
            {
                if (dataSaver.IsDirty)
                {
                    Save(dataSaver);
                }
            }
            //
            _timer = 0f;
        }

        public void Save(IDataSaver saver)
        {
            var path = $"{SavePath}/{saver.DataFile}";
            using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Write);
            if (Godot.FileAccess.GetOpenError() != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }
            //
            var data = new Dictionary<string, string>();
            saver.WriteData(data);
            file.StoreString(JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        #endregion IDataSaverService


        public override void _Ready()
        {
            if (Godot.DirAccess.DirExistsAbsolute(SavePath))
            {
                return;
            }

            var ok = Godot.DirAccess.MakeDirRecursiveAbsolute(SavePath);
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to create folder {SavePath}");
            }
        }

        public override void _Process(double delta)
        {
            _timer += delta;
            if (_timer < SaveTimer)
            {
                return;
            }

            Save();
        }
    }
}
*/

using Godot;

namespace Lavos.Services.Data
{
    sealed partial class DataSaverService
        : Node
        , IDataSaverService
    {
        void IDataSaverService.Register(IDataSaver saver) { }
        void IDataSaverService.Unregister(IDataSaver saver) { }

        void IDataSaverService.CleanData() { }

        void IDataSaverService.Load() { }
        void IDataSaverService.Load(IDataSaver saver) { }

        void IDataSaverService.Save() { }
        void IDataSaverService.Save(IDataSaver saver) { }
    }
}