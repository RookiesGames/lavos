using Lavos.Core;
using Lavos.Dependency;
using System;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class StackStateMachine : IStackStateMachine
{
    IStackState _pendingState;
    readonly Stack<IStackState> _stateStack = new();

    public StackStateMachine(IStackState initialState)
    {
        var service = ServiceLocator.Locate<IProcessorService>();
        service.Register(this);
        //
        initialState.Phase = StackStatePhase.Pushing;
        _stateStack.Push(initialState);
    }

    void IDisposable.Dispose()
    {
        var service = ServiceLocator.Locate<IProcessorService>();
        service.Unregister(this);
        //
        _pendingState = null;
        _stateStack.Clear();
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
                        peek.Phase = StackStatePhase.Popped;
                        peek.Exit();
                        break;
                    }
                case StackStatePhase.Popping:
                    {
                        peek.Phase = StackStatePhase.Exiting;
                        peek.Pause();
                        break;
                    }
                case StackStatePhase.Pushing:
                    {
                        peek.StatePushed += OnStatePushed;
                        peek.StatePopped += OnStatePopped;
                        //
                        peek.Phase = StackStatePhase.Resuming;
                        peek.Enter();
                        break;
                    }
                case StackStatePhase.Resuming:
                    {
                        peek.Phase = StackStatePhase.Running;
                        peek.Resume();
                        break;
                    }
                case StackStatePhase.Running:
                    {
                        peek.Update(delta);
                        break;
                    }
                case StackStatePhase.Pausing:
                    {
                        peek.Phase = StackStatePhase.Paused;
                        peek.Pause();
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