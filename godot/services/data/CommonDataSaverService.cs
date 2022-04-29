using Godot;
using Lavos.Console;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Lavos.Services.Data
{
    sealed class CommonDataSaverService
        : Node
        , IDataSaverService
    {
        const string Tag = nameof(CommonDataSaverService);
        const string SavePath = "user://saves";
        const float SaveTimer = 1f;

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
            // delete SavePath/*
            var dir = new Godot.Directory();
            var ok = dir.Open(SavePath);
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open directory {SavePath}");
                return;
            }

            ok = dir.ListDirBegin();
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to list directory {SavePath}");
                return;
            }

            var wd = dir.GetCurrentDir();
            while (true)
            {
                var file = dir.GetNext();
                if (string.IsNullOrEmpty(file))
                {
                    break;
                }

                var fullpath = "";
                ok = dir.Remove(fullpath);
                if (ok != Error.Ok)
                {
                    Log.Error(Tag, $"Failed to remove file {fullpath}");
                    return;
                }
            }
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
            var ok = file.Open(path, Godot.File.ModeFlags.Read);
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }

            var content = file.GetAsText();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            if (data != null) { saver.LoadData(data); }
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
            var ok = file.Open(path, Godot.File.ModeFlags.Write);
            if (ok != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}");
                return;
            }
            //
            saver.WriteData();
            var json = JsonConvert.SerializeObject(saver.Data);
            file.StoreString(json);
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