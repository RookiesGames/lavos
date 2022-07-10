using Lavos.Dependency;

namespace Lavos.Services.Data
{
    public interface IDataSaverService : IService
    {
        void Register(IDataSaver saver);
        void Unregister(IDataSaver saver);

        void CleanData();

        void Load();
        void Load(IDataSaver saver);

        void Save();
        void Save(IDataSaver saver);
    }
}