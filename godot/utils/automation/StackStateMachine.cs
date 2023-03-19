using Lavos.Debug;
using System.Collections.Generic;

namespace Lavos.Utils.Automation
{
    public sealed class StackStateMachine
    {
        Stack<IStackState> _stateStack = null;

        public void PushState(IStackState state)
        {
            var peek = _stateStack.Peek();
            if (peek != null)
            {
                peek.Phase = StackStatePhase.Pausing;
            }

            state.Phase = StackStatePhase.Pushing;
            _stateStack.Push(state);
        }

        public void PopState()
        {
            var peek = _stateStack.Peek();
            peek.Phase = StackStatePhase.Popping;
        }

        public void PushAndPopState(IStackState state)
        {
            var peek = _stateStack.Peek();
            peek.Phase = StackStatePhase.Popping;

            state.Phase = StackStatePhase.Pushing;
            _stateStack.Push(state);
        }

        public void Process(double dt)
        {
            var peek = _stateStack.Peek();
            if (peek == null)
            {
                return;
            }
            //
            if (peek.Phase == StackStatePhase.Popped)
            {
                _stateStack.Pop();
                peek = _stateStack.Peek();
                if (peek != null)
                {
                    peek.Phase = StackStatePhase.Resuming;
                }
            }
            //
            foreach (var state in _stateStack)
            {
                switch (state.Phase)
                {
                    case StackStatePhase.Popped: break;
                    case StackStatePhase.Popping:
                        {
                            state.Pause();
                            state.Exit();
                            state.Phase = StackStatePhase.Popped;
                            break;
                        };
                    case StackStatePhase.Pushing:
                        {
                            state.Enter();
                            state.Resume();
                            state.Phase = StackStatePhase.Running;
                            break;
                        }
                    case StackStatePhase.Resuming:
                        {
                            state.Resume();
                            state.Phase = StackStatePhase.Running;
                            break;
                        }
                    case StackStatePhase.Running:
                        {
                            state.Process(dt);
                            break;
                        };
                    case StackStatePhase.Pausing:
                        {
                            state.Pause();
                            state.Phase = StackStatePhase.Paused;
                            break;
                        }
                    case StackStatePhase.Paused: break;
                }
            }
        }
    }
}