using System.Collections.Generic;
using System.Linq;

namespace Identity.Service.ldap.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> newValues)
        {
            var arr = newValues.ToArray();

            foreach (var value in arr)
            {
                source.Add(value);
            }
        }
    }
}
