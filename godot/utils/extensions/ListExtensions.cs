using Godot;
using System.Collections.Generic;

namespace Lavos.Utils.Extensions;

public static class ListExtensions
{
    public static bool IsEmpty<T>(this List<T> list) => list.Count == 0;
    public static bool IsNotEmpty<T>(this List<T> list) => list.Count > 0;

    public static T First<T>(this List<T> list) => (list.Count == 0) ? default(T) : list[0];

    public static bool PushUnique<T>(this List<T> list, T item)
    {
        if (list.Contains(item))
        {
            return false;
        }
        //
        list.Add(item);
        return true;
    }

    public static bool InsertUnique<T>(this List<T> list, int index, T item)
    {
        if (list.Contains(item))
        {
            return false;
        }
        //
        index = Mathf.Clamp(index, 0, list.Count);
        list.Insert(index, item);
        return true;
    }
}
