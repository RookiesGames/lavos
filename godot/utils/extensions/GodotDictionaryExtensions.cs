using Godot;
using Godot.Collections;

namespace Lavos.Utils.Extensions;

public static class GodotDictionaryExtensions
{
    public static bool DoesNotContainKey<[MustBeVariant] T, [MustBeVariant] U>(this Dictionary<T, U> dic, T key)
    {
        return dic.ContainsKey(key) == false;
    }

    public static U GetOrDefault<[MustBeVariant] T, [MustBeVariant] U>(this Dictionary<T, U> dic, T key, U fallback = default)
    {
        if (dic.DoesNotContainKey<T, U>(key))
        {
            return fallback;
        }
        //
        return dic[key];
    }

    public static void SetOrAdd<[MustBeVariant] T, [MustBeVariant] U>(this Dictionary<T, U> dic, T key, U value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        //
        dic.Add(key, value);
    }
}