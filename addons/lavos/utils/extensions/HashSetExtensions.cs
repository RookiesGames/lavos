using System;
using System.Collections.Generic;

namespace Lavos.Utils.Extensions;

public static class HashSetExtensions
{
    public static void ForEach<T>(this HashSet<T> hashSet, Action<T> predicate)
    {
        foreach (var elt in hashSet)
        {
            predicate?.Invoke(elt);
        }
    }
}