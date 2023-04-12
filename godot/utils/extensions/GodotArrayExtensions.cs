using Godot;
using Godot.Collections;

namespace Lavos.Utils.Extensions;

public static class GodotArrayExtensions
{
    public static bool IsEmpty<[MustBeVariant] T>(this Array<T> array) => array.Count == 0;
    public static bool IsNotEmpty<[MustBeVariant] T>(this Array<T> array) => array.Count > 0;

    public static T First<[MustBeVariant] T>(this Array<T> array) => (array.Count == 0) ? default(T) : array[0];

    public static bool PushUnique<[MustBeVariant] T>(this Array<T> array, T item)
    {
        if (array.Contains(item))
        {
            return false;
        }
        //
        array.Add(item);
        return true;
    }

    public static bool InsertUnique<[MustBeVariant] T>(this Array<T> array, int index, T item)
    {
        if (array.Contains(item))
        {
            return false;
        }
        //
        index = Mathf.Clamp(index, 0, array.Count);
        array.Insert(index, item);
        return true;
    }
}