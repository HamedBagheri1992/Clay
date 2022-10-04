using System.Collections.Generic;

namespace SharedKernel.Extensions
{
    public static class CollectionExtension
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            if (source != null)
                foreach (T item in source)
                {
                    destination.Add(item);
                }
        }


        public static void RemoveRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            if (source != null)
                foreach (T item in source)
                {
                    destination.Remove(item);
                }
        }
    }
}
