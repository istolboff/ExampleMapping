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

        public static void RemoveIf<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            Contract.Requires(@this != null);

            @this.RemoveRange(@this.Where(predicate).AsImmutable());
        }

        private static void RemoveRange<T>(this ICollection<T> @this, IEnumerable<T> elementsToRemove)
        {
            foreach (var element in elementsToRemove)
            {
                @this.Remove(element);
            }
        }
    }
}
