
namespace Lavos.Input;

public interface IGamepadInputHandler : IInputHandler<IGamepadInputConfig>
{
    void RegisterListener(IGamepadInputEventListener listener);
    void UnregisterListener(IGamepadInputEventListener listener);

    void EnableHandler(GamepadDevice device, IGamepadInputConfig config);
}