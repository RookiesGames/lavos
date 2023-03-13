using System.Collections.Generic;

namespace Lavos.Utils.Automation
{
    public sealed class StackStateMachine
    {
        Stack<IStackState> _stateStack = null;

        public void PushState(IStackState state)
        {
            var peek = _stateStack.Peek();
            peek?.Pause?.Invoke();
            //
            _stateStack.Push(state);
            state?.Enter?.Invoke();
        }

        public void PopState()
        {
            var toPop = _stateStack.Pop();
            toPop?.Pause?.Invoke();
            //
            var peek = _stateStack.Peek();
            peek?.Resume?.Invoke();
            //
            toPop?.Exit?.Invoke();
        }

        public void PushAndPopState(IStackState state)
        {
            var peek = _stateStack.Pop();
            peek?.Pause?.Invoke();
            //
            _stateStack.Push(state);
            state?.Enter?.Invoke();
            //
            peek?.Exit?.Invoke();
        }

        public void Process(double dt)
        {
            var state = _stateStack.Peek();
            state.Process?.Invoke(dt);
        }
    }
}