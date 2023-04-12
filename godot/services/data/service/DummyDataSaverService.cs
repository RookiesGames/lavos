using System.Collections.Generic;

namespace Lavos.Services.Data;

sealed class DummyDataSaverService : IDataSaverService
{
    const string Tag = nameof(DummyDataSaverService);

    List<IDataSaver> _dataSavers = new List<IDataSaver>();

    public void Register(IDataSaver saver)
    {
        _dataSavers.PushUnique(saver);
        Log.Debug(Tag, $"{saver.DataFile} registered");
    }

    public void Unregister(IDataSaver saver)
    {
        _dataSavers.Remove(saver);
        Log.Debug(Tag, $"{saver.DataFile} unregistered");
    }

    public T GetDataSaver<T>() where T : IDataSaver => default(T);

    public void CleanData()
    {
        Log.Debug(Tag, "Data removed");
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
        Log.Debug(Tag, $"File {saver.DataFile} loaded");
    }

    public void Save()
    {
        foreach (var dataSaver in _dataSavers)
        {
            Save(dataSaver);
        }
    }

    public void Save(IDataSaver saver)
    {
        Log.Debug(Tag, $"File {saver.DataFile} saved");
    }
}
