using System;
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

        public static T SingleOrThrow<T>(
            this IEnumerable<T> @this, 
            Func<Exception> createCollectionIsEmptyException, 
            Func<Exception> createCollectionContrainsMoreThanOneElementException)
        {
            Contract.Requires(@this != null);
            Contract.Requires(createCollectionIsEmptyException != null);
            Contract.Requires(createCollectionContrainsMoreThanOneElementException != null);

            var result = default(T);

            var count = 0;
            foreach (var item in @this)
            {
                ++count;
                if (count > 1)
                {
                    throw createCollectionContrainsMoreThanOneElementException();
                }

                result = item;
            }

            if (count == 0)
            {
                throw createCollectionIsEmptyException();
            }

            return result;
        }
    }
}
