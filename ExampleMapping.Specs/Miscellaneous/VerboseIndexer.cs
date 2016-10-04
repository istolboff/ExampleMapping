using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal sealed class VerboseIndexer<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public VerboseIndexer(string dictionaryName, IDictionary<TKey, TValue> dictionary)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(dictionaryName));
            Contract.Requires(dictionary != null);

            _dictionaryName = dictionaryName;
            _dictionary = dictionary;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue result;
                var keyExists = _dictionary.TryGetValue(key, out result);
                Verify.That(
                    keyExists,
                    () => $"Dictionary {_dictionaryName} does not contain key '{key}'. " +
                          $"Only following keys are present: [{string.Join(", ", _dictionary.Keys)}].");

                return _dictionary[key];
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly string _dictionaryName;
        private readonly IDictionary<TKey, TValue> _dictionary;
    }
}
