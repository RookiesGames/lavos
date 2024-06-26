using System.Collections.Generic;

namespace Lavos.Utils.Extensions;

public static class DictionaryExtensions
{
    public static bool DoesNotContainKey<T, U>(this Dictionary<T, U> dic, T key)
    {
        return !dic.ContainsKey(key);
    }

    public static U GetOrDefault<T, U>(this Dictionary<T, U> dic, T key, U fallback = default)
    {
        if (dic.DoesNotContainKey(key))
        {
            return fallback;
        }
        //
        return dic[key];
    }

    public static void SetOrAdd<T, U>(this Dictionary<T, U> dic, T key, U value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
            return;
        }
        //
        dic.Add(key, value);
    }

    public static void Merge<T, U>(this Dictionary<T, U> dic, Dictionary<T, U> other)
    {
        foreach (var kvp in other)
        {
            dic.SetOrAdd(kvp.Key, kvp.Value);
        }
    }
}
