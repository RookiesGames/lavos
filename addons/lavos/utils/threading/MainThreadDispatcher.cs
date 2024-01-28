using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Utils.Threading;

// Untested
public sealed class MainThreadDispatcher : IThreadDispatcher
{
    readonly object _lock;
    readonly int _threadId;
    readonly List<Action> _pendingActions;

    public MainThreadDispatcher()
    {
        _lock = new();
        _pendingActions = [];
        _threadId = Environment.CurrentManagedThreadId;
    }

    public void AddAction(Action action)
    {
        if (_threadId == Environment.CurrentManagedThreadId)
        {
            action.Invoke();
        }
        else
        {
            lock (_lock)
            {
                _pendingActions.Add(action);
            }
        }
    }

    void Run()
    {
        Task.Run(async () =>
        {
            var listCopy = new List<Action>();
            while (true)
            {
                if (_pendingActions.Count > 0)
                {
                    lock (_lock)
                    {
                        listCopy.AddRange(_pendingActions);
                        _pendingActions.Clear();
                    }

                    foreach (var action in listCopy)
                    {
                        action.Invoke();
                    }
                    listCopy.Clear();
                }
                await Task.Delay(TimeSpan.FromMilliseconds(16));
            }
        });
    }
}
