using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExampleMapping.Specs.Miscellaneous;
using ExampleMapping.Specs.WebSut.WatinExtensions;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut
{
    internal sealed class NewlyCreatedElementFinder<TElement> where TElement : Element
    {
        public NewlyCreatedElementFinder(IElementContainer browser, Regex elementIdRegex)
        {
            _browser = browser;
            _elementIdRegex = elementIdRegex;
            _knownElements = browser.Elements<TElement>(elementIdRegex).AsImmutable();
        }

        public TElement Result
        {
            get
            {
                var newElements = Wait.Until(
                    () => _browser.Elements<TElement>(_elementIdRegex).Except(_knownElements),
                    newMatchingElements => newMatchingElements.Any(),
                    TimeSpan.FromSeconds(3),
                    _ => new TimeoutException($"No new elements with Id or Name matching Regex '{_elementIdRegex}' has appeared."))
                    .AsImmutable();

                return newElements.SingleOrThrow(
                    createCollectionContrainsMoreThanOneElementException: () => new InvalidOperationException(
                        $"We expected a single new element with Id or Name matching Regex '{_elementIdRegex}' to appear, but in fact several of them did: [{string.Join(", ", newElements)}]"),
                    createCollectionIsEmptyException: () => new InvalidOperationException("If this exception is thrown, then there is a logic error in test."));
            }
        }

        private readonly IElementContainer _browser;
        private readonly Regex _elementIdRegex;
        private readonly IReadOnlyCollection<TElement> _knownElements;
    }
}
