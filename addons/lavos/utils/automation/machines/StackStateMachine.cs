using Lavos.Core;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class StackStateMachine : IProcessable
{
    StackState _pendingState;
    readonly Stack<StackState> _stateStack = new();

    public StackState CurrentState => _stateStack.Peek();

    public void Process(double delta)
    {
        if (_stateStack.TryPeek(out StackState peek))
        {
            switch (peek.Phase)
            {
                case StatePhase.Popped:
                    {
                        _stateStack.Pop();
                        break;
                    }
                case StatePhase.Exiting:
                    {
                        peek.Phase = StatePhase.Popped;
                        peek.Exit();
                        break;
                    }
                case StatePhase.Popping:
                    {
                        peek.Phase = StatePhase.Exiting;
                        peek.Pause();
                        break;
                    }
                case StatePhase.Pushing:
                    {
                        peek.Phase = StatePhase.Resuming;
                        peek.Enter();
                        break;
                    }
                case StatePhase.Resuming:
                    {
                        peek.Phase = StatePhase.Running;
                        peek.Resume();
                        break;
                    }
                case StatePhase.Running:
                    {
                        peek.Update(delta);
                        break;
                    }
                case StatePhase.Pausing:
                    {
                        peek.Phase = StatePhase.Paused;
                        peek.Pause();
                        break;
                    }
                case StatePhase.Paused:
                    {
                        if (_pendingState != null)
                        {
                            _pendingState.Phase = StatePhase.Pushing;
                            _stateStack.Push(_pendingState);
                            //
                            _pendingState = null;
                        }
                        else
                        {
                            peek.Phase = StatePhase.Resuming;
                        }
                        break;
                    }
            }
        }
    }

    public void PushState<T>() where T : StackState, new()
    {
        PushState(new T());
    }

    public void PushState(StackState state)
    {
        _pendingState = state;
        _pendingState.StateMachine = this;
        //
        if (_stateStack.TryPeek(out StackState peek))
        {
            peek.Phase = StatePhase.Pausing;
        }
        else
        {
            _pendingState.Phase = StatePhase.Pushing;
            _stateStack.Push(_pendingState);
        }
    }

    public void PopState()
    {
        if (_stateStack.TryPeek(out StackState peek))
        {
            peek.Phase = StatePhase.Popping;
        }
    }
}
