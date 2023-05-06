using Lavos.Core;
using System;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class StackStateMachine : IStackStateMachine
{
    IStackState _pendingState;
    readonly Stack<IStackState> _stateStack = new();

    public StackStateMachine(IStackState initialState)
    {
        initialState.Phase = StackStatePhase.Pushing;
        _stateStack.Push(initialState);
    }

    #region IStackStateMachine

    IStackState IStackStateMachine.CurrentState => _stateStack.Peek();

    void IProcessable.Process(double delta)
    {
        if (_stateStack.TryPeek(out IStackState peek))
        {
            switch (peek.Phase)
            {
                case StackStatePhase.Popped:
                    {
                        _stateStack.Pop();
                        break;
                    }
                case StackStatePhase.Exiting:
                    {
                        peek.Exit();
                        peek.Phase = StackStatePhase.Popped;
                        break;
                    }
                case StackStatePhase.Popping:
                    {
                        peek.Pause();
                        peek.Phase = StackStatePhase.Exiting;
                        break;
                    }
                case StackStatePhase.Pushing:
                    {
                        peek.StatePushed += OnStatePushed;
                        peek.StatePopped += OnStatePopped;
                        //
                        peek.Enter();
                        peek.Phase = StackStatePhase.Resuming;
                        break;
                    }
                case StackStatePhase.Resuming:
                    {
                        peek.Resume();
                        peek.Phase = StackStatePhase.Running;
                        break;
                    }
                case StackStatePhase.Running:
                    {
                        peek.Process(delta);
                        break;
                    }
                case StackStatePhase.Pausing:
                    {
                        peek.Pause();
                        peek.Phase = StackStatePhase.Paused;
                        break;
                    }
                case StackStatePhase.Paused:
                    {
                        if (_pendingState != null)
                        {
                            _pendingState.Phase = StackStatePhase.Pushing;
                            _stateStack.Push(_pendingState);
                            //
                            _pendingState = null;
                        }
                        else
                        {
                            peek.Phase = StackStatePhase.Resuming;
                        }
                        break;
                    }
            }
        }
    }

    void IStackStateMachine.PushState(IStackState state)
    {
        OnStatePushed(state);
    }

    void OnStatePushed(IStackState state)
    {
        if (_stateStack.TryPeek(out IStackState peek))
        {
            peek.Phase = StackStatePhase.Pausing;
        }
        //
        _pendingState = state;
    }

    void IStackStateMachine.PopState()
    {
        OnStatePopped();
    }

    void OnStatePopped()
    {
        if (_stateStack.TryPeek(out IStackState peek))
        {
            peek.StatePushed -= OnStatePushed;
            peek.StatePopped -= OnStatePopped;
            peek.Phase = StackStatePhase.Popping;
        }
    }

    #endregion
}
