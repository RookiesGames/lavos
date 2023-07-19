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
    readonly List<IDataSaver> _dataSavers = new();

    #region IDataSaverService

    public void Register(IDataSaver saver)
    {
        _dataSavers.PushUnique(saver);
    }

    public void Unregister(IDataSaver saver)
    {
        _dataSavers.Remove(saver);
    }

    public T GetDataSaver<T>() where T : IDataSaver => (T)_dataSavers.Find(s => s.GetType() == typeof(T));

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

    public void Load(IDataSaver saver)
    {
        var path = System.IO.Path.Combine(SavePath, saver.DataFile);
        var flag = FileAccess.FileExists(path)
                            ? FileAccess.ModeFlags.Read
                            : FileAccess.ModeFlags.WriteRead;
        using var file = FileAccess.Open(path, flag);
        if (FileAccess.GetOpenError() != Error.Ok)
        {
            Log.Error(Tag, $"Failed to open file {path}");
            return;
        }
        Log.Debug(Tag, $"Loading data saver: {file.GetPathAbsolute()}");
        //
        var content = file.GetAsText();
        using var json = new Json();
        var parseResult = json.Parse(content);
        if (parseResult == Error.Ok)
        {
            var data = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
            saver.LoadData(data);
        }
    }

    public void Save(IDataSaver saver)
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
        _timer = 0f;
    }

    private void Save()
    {
        foreach (var dataSaver in _dataSavers)
        {
            if (dataSaver.IsDirty)
            {
                Save(dataSaver);
                dataSaver.ClearFlag();
            }
        }
    }
}
