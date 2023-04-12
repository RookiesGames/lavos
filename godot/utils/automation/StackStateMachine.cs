using Lavos.Core;
using System;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class StackStateMachine : IStackStateMachine
{
    IStackState _pendingState = null;
    Stack<IStackState> _stateStack = new Stack<IStackState>();

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
                case StackStatePhase.Popping:
                    {
                        peek.Pause();
                        peek.Exit();
                        //
                        peek.StatePushed -= OnStatePushed;
                        peek.StatePopped -= OnStatePopped;
                        //
                        peek.Phase = StackStatePhase.Popped;
                        break;
                    }
                case StackStatePhase.Pushing:
                    {
                        peek.StatePushed += OnStatePushed;
                        peek.StatePopped += OnStatePopped;
                        //
                        peek.Enter();
                        peek.Resume();
                        //
                        peek.Phase = StackStatePhase.Running;
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
                    };
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
        OnStatePushed(this, state);
    }

    void OnStatePushed(object sender, IStackState state)
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
        OnStatePopped(this, null);
    }

    void OnStatePopped(object sender, EventArgs e)
    {
        if (_stateStack.TryPeek(out IStackState peek))
        {
            peek.Phase = StackStatePhase.Popping;
        }
    }

    #endregion
}
