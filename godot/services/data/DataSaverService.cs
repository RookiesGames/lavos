using Godot;
using Lavos.Console;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lavos.Services.Data
{
    sealed class DataSaverService
        : Node
        , IDataSaverService
    {
        const string Tag = nameof(DataSaverService);
        const string SavePath = "user://saves";
        const float SaveTimer = 0.25f;

        float _timer = 0f;
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
            var dir = new Godot.Directory();
            var ok = dir.Open(SavePath);
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open directory {SavePath}");
                return;
            }

            dir.RemoveDirectory(SavePath);
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
            var file = new Godot.File();
            var error = file.Open(path, Godot.File.ModeFlags.Read);
            if (error == Error.FileNotFound)
            {
                error = file.Open(path, Godot.File.ModeFlags.WriteRead);
            }
            //
            if (error != Error.Ok)
            {

                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }
            //
            var content = file.GetAsText();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            saver.LoadData(data);
            file.Close();
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
            var file = new Godot.File();
            var error = file.Open(path, Godot.File.ModeFlags.Write);
            if (error != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }
            //
            var data = new Dictionary<string, string>();
            saver.WriteData(data);
            file.StoreString(JsonConvert.SerializeObject(data, Formatting.Indented));
            file.Close();
        }

        #endregion IDataSaverService


        public override void _Ready()
        {
            var dir = new Godot.Directory();
            if (!dir.DirExists(SavePath))
            {
                var ok = dir.MakeDir(SavePath);
                if (ok != Error.Ok)
                {
                    Log.Error(Tag, $"Failed to create folder {SavePath}");
                }
            }
        }

        public override void _Process(float delta)
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