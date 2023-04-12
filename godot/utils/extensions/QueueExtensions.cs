using System.Collections.Generic;

namespace Lavos.Utils.Extensions;

public static class QueueExtensions
{
    public static void EnqueueUnique<T>(this Queue<T> queue, T element)
    {
        if (queue.Contains(element) == false)
        {
            queue.Enqueue(element);
        }
    }
}
