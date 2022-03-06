using System.Collections.Generic;

namespace Lavos.Utils.Extensions
{
    public static class ListExtensions
    {
        public static T First<T>(this List<T> list)
        {
            return (list.Count == 0) ? default(T) : list[0];
        }

        public static void PushUnique<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Add(item);
            }
        }
    }
}