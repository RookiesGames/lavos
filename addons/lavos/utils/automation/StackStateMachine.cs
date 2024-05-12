using Lavos.Core;
using Lavos.Dependency;
using System;
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

    public void PushState(StackState state)
    {
        _pendingState = state;
        _pendingState.StateMachine = this;
        //
        if (_stateStack.TryPeek(out StackState peek))
        {
            peek.Phase = StackStatePhase.Pausing;
        }
        else
        {
            _pendingState.Phase = StackStatePhase.Pushing;
            _stateStack.Push(_pendingState);
        }
    }

    public void PopState()
    {
        if (_stateStack.TryPeek(out StackState peek))
        {
            peek.Phase = StackStatePhase.Popping;
        }
    }
}
