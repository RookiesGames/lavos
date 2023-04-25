using Godot;
using Godot.Collections;

namespace Lavos.Services.Data;

public abstract class DataSaver : IDataSaver
{
    #region IDataSaver

    bool _isDirty = false;
    public bool IsDirty => _isDirty;

    public string DataFile => "data";
    public Dictionary<string, Variant> Data { get; } = new();

    public void LoadData(Dictionary<string, Variant> data)
    {
        Data.Clear();
        Data.Merge(data);
    }

    public void SaveData(string key, Variant data)
    {
        Data.SetOrAdd<string, Variant>(key, data);
        _isDirty = true;
    }

    public void CleanData()
    {
        Data.Clear();
        _isDirty = true;
    }

    public void ClearFlag() => _isDirty = false;

    #endregion
}