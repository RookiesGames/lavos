using Godot;
using Godot.Collections;

namespace Lavos.Services.Data;

public abstract class DataSaver : IDataSaver
{
    #region IDataSaver

    private bool _isDirty = false;
    public bool IsDirty => _isDirty;

    public string DataFile => "data";

    private Dictionary<string, Variant> _data = new Dictionary<string, Variant>();
    public Dictionary<string, Variant> Data => _data;

    public void LoadData(Dictionary<string, Variant> data)
    {
        _data.Clear();
        _data.Merge(data);
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