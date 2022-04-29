using Lavos.Console;

namespace Lavos.Services.Data
{
    sealed class DummyDataSaverService : IDataSaverService
    {
        const string Tag = nameof(DummyDataSaverService);


        public void Register(IDataSaver saver)
        {
            Log.Debug(Tag, $"{saver.DataFile} registered");
        }

        public void Unregister(IDataSaver saver)
        {
            Log.Debug(Tag, $"{saver.DataFile} unregistered");
        }

        public void CleanData()
        {
            Log.Debug(Tag, "Data removed");
        }
    }
}