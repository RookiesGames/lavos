
namespace Lavos.Input
{
    public interface IGamepadInputHandler
        : IInputHandler<IGamepadInputConfig>
    {
        void RegisterListener(IGamepadInputListener listener);
        void UnregisterListener(IGamepadInputListener listener);

        void EnableHandler(GamepadDevice device, IGamepadInputConfig config);
    }
}