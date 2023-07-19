
namespace Lavos.Input;

public interface IMouseInputHandler
    : IInputHandler<IMouseInputConfig>
{
    void RegisterListener(IMouseInputListener listener);
    void UnregisterListener(IMouseInputListener listener);
}
