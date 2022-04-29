using System.Collections.Generic;

namespace Lavos.Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool DoesNotContainKey<T, U>(this Dictionary<T, U> dic, T key)
        {
            return dic.ContainsKey(key) == false;
        }

        public static bool DoesNotContainValue<T, U>(this Dictionary<T, U> dic, U value)
        {
            return dic.ContainsValue(value) == false;
        }

        public static void SetEntry<T, U>(this Dictionary<T, U> dic, T key, U value)
        {
            if (dic.DoesNotContainKey(key))
            {
                dic.Add(key, value);
            }
            else
            {
                dic[key] = value;
            }
        }
    }
}