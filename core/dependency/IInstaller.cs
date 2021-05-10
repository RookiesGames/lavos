
namespace Vortico.Core.Dependency
{
    public interface IInstaller
    {
        void Install(IDependencyContainer container);
        void Initialize();
    }
}