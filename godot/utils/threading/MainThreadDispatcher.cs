using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Utils.Threading;

// Untested
public sealed class MainThreadDispatcher : IThreadDispatcher
{
    readonly object _lock = new object();
    int _threadId = int.MinValue;
    List<Action> _pendingActions = new List<Action>();

    public MainThreadDispatcher()
    {
        _threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    public void AddAction(Action action)
    {
        if (_threadId == System.Threading.Thread.CurrentThread.ManagedThreadId)
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
