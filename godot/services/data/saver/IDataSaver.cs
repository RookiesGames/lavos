using Godot;
using Godot.Collections;

namespace Lavos.Services.Data;

public interface IDataSaver
{
    bool IsDirty { get; }
    string DataFile { get; }
    Dictionary<string, Variant> Data { get; }

    void LoadData(Dictionary<string, Variant> data);
    void SaveData(string key, Variant data);
    Variant GetData(string key);

    void CleanData();
    void ClearFlag();
}
