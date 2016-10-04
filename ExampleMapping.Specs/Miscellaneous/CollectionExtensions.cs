using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal static class CollectionExtensions
    {
        public static IReadOnlyCollection<T> AsImmutable<T>(this IEnumerable<T> @this)
        {
            Contract.Requires(@this != null);

            return (@this as IReadOnlyCollection<T>) ?? @this.ToList();
        }

        public static VerboseIndexer<TKey, TValue> WithVerboseIndexing<TKey, TValue>(this IDictionary<TKey, TValue> @this, string dictionaryName)
        {
            Contract.Requires(@this != null);

            return new VerboseIndexer<TKey, TValue>(dictionaryName, @this);
        }
    }
}
