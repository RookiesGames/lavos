
namespace Lavos.Services.Data
{
    public interface IDataSaverService
    {
        void Register(IDataSaver saver);
        void Unregister(IDataSaver saver);

        void CleanData();
    }
}