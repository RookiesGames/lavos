
namespace Vortico.Utils.Automation
{
    public sealed class StateMachine
    {
        private IState _state = null;

        public void ChangeState(IState state)
        {
            _state?.Clean?.Invoke();
            _state = state;
            _state?.Ready?.Invoke();
        }

        public void Process()
        {
            _state?.Process.Invoke();
        }
    }
}