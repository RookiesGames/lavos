using Godot;
using System.Collections.Generic;

namespace Lavos.Services.Data;

sealed partial class DataSaverService
    : Node
    , IDataSaverService
{
    const string Tag = nameof(DataSaverService);
    const string SavePath = "user://saves";
    const float SaveTimer = 0.25f;

    double _timer = 0;
    readonly HashSet<IDataSaver> _dataSavers = new();

    #region IDataSaverService

    public void Register(IDataSaver saver)
    {
        _dataSavers.Add(saver);
    }

    public void Unregister(IDataSaver saver)
    {
        _dataSavers.Remove(saver);
    }

    public T GetDataSaver<T>() where T : IDataSaver
    {
        foreach (var dataSaver in _dataSavers)
        {
            if (dataSaver.GetType() == typeof(T))
            {
                return (T)dataSaver;
            }
        }
        Assert.Fail($"Could not find data saver of type {typeof(T)}");
        return default;
    }

    public void CleanData()
    {
        var dir = DirAccess.Open(SavePath);
        if (DirAccess.GetOpenError() != Error.Ok)
        {
            Log.Error(Tag, $"Failed to open directory {SavePath}");
            return;
        }

        DirAccess.RemoveAbsolute(dir.GetCurrentDir());
    }

    public void ReadData(IDataSaver saver)
    {
        var path = System.IO.Path.Combine(SavePath, saver.DataFile);
        var flag = FileAccess.ModeFlags.WriteRead;
        using var file = FileAccess.Open(path, flag);
        {
            var error = FileAccess.GetOpenError();
            if (error != Error.Ok)
            {
                Log.Error(Tag, $"Failed to open file {path}. {error}");
                return;
            }
            //
            Log.Debug(Tag, $"Loading data saver: {file.GetPathAbsolute()}");
            //
            var data = new Godot.Collections.Dictionary<string, Variant>();
            var content = file.GetAsText();
            if (content.IsNotNullOrEmpty())
            {
                using var json = new Json();
                var parseResult = json.Parse(content);
                if (parseResult == Error.Ok)
                {
                    data.Merge(json.Data.AsGodotDictionary<string, Variant>());
                }
                else
                {
                    Log.Warn(Tag, $"Failed to parse content. {parseResult}");
                }
            }
            saver.LoadData(data);
        }
    }

    public void WriteData(IDataSaver saver)
    {
        var path = System.IO.Path.Combine(SavePath, saver.DataFile);
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        if (FileAccess.GetOpenError() != Error.Ok)
        {
            Log.Error(Tag, $"Failed to open file {path}");
            return;
        }
        //
        var data = saver.Data;
        file.StoreString(Json.Stringify(data));
    }

    #endregion IDataSaverService

    public override void _Ready()
    {
        if (DirAccess.DirExistsAbsolute(SavePath))
        {
            return;
        }

        var ok = DirAccess.MakeDirRecursiveAbsolute(SavePath);
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
        _timer = 0f;
    }

    private void Save()
    {
        foreach (var dataSaver in _dataSavers)
        {
            if (dataSaver.IsDirty)
            {
                WriteData(dataSaver);
                dataSaver.ClearFlag();
            }
        }
    }
}
