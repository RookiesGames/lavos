using System.Collections.Generic;

namespace Lavos.Services.Data;

sealed class DummyDataSaverService : IDataSaverService
{
    const string Tag = nameof(DummyDataSaverService);

    readonly HashSet<IDataSaver> _dataSavers = new();

    public void Register(IDataSaver saver)
    {
        _dataSavers.Add(saver);
        Log.Debug(Tag, $"{saver.DataFile} registered");
    }

    public void Unregister(IDataSaver saver)
    {
        _dataSavers.Remove(saver);
        Log.Debug(Tag, $"{saver.DataFile} unregistered");
    }

    public T GetDataSaver<T>() where T : IDataSaver => default;

    public void CleanData()
    {
        Log.Debug(Tag, "Data removed");
    }

    public void ReadData(IDataSaver saver)
    {
        Log.Debug(Tag, $"File {saver.DataFile} loaded");
    }

    public void WriteData(IDataSaver saver)
    {
        Log.Debug(Tag, $"File {saver.DataFile} saved");
    }
}
