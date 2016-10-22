using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ExampleMapping.Web.Miscellaneous
{
    internal static class CollectionExtensionscs
    {
        public static IReadOnlyCollection<T> AsImmutable<T>(this IEnumerable<T> @this)
        {
            Contract.Requires(@this != null);

            return @this as IReadOnlyCollection<T> ?? @this.ToList();
        }

        public static void RemoveRange<T>(this ICollection<T> @this, IReadOnlyCollection<T> elementsToRemove)
        {
            foreach (var element in elementsToRemove)
            {
                @this.Remove(element);
            }
        }

        public static void RemoveIf<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            @this.RemoveRange(@this.Where(predicate).AsImmutable());
        }
    }
}
