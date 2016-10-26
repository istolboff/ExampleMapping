using System;
using System.Collections.Generic;
using System.Linq;
using ExampleMapping.Specs.Miscellaneous;
using WatiN.Core;
using WatiN.Core.Constraints;
using ExampleMapping.Specs.WebSut.WatinExtensions;

namespace ExampleMapping.Specs.WebSut
{
    internal sealed class NewlyCreatedElementFinder<TElement> where TElement : Element
    {
        public NewlyCreatedElementFinder(IElementContainer browser, Constraint elementConstraint)
        {
            _browser = browser;
            _elementConstraint = elementConstraint;
            _knownElements = browser.Elements<TElement>(elementConstraint).AsImmutable();
        }

        public TElement Result
        {
            get
            {
                var newElements = Wait.Until(
                    () => _browser.Elements<TElement>(_elementConstraint).Except(_knownElements),
                    newMatchingElements => newMatchingElements.Any(),
                    TimeSpan.FromSeconds(3),
                    _ => new TimeoutException($"No new elements of type {typeof(TElement).Name} matching constraint '{_elementConstraint}' has appeared."))
                    .AsImmutable();

                return newElements.SingleOrThrow(
                    createCollectionContrainsMoreThanOneElementException: () => new InvalidOperationException(
                        $"We expected a single new element of type {typeof(TElement).Name} matching constraint '{_elementConstraint}' to appear, but in fact several of them did: [{string.Join(", ", newElements)}]"),
                    createCollectionIsEmptyException: () => new InvalidOperationException("If this exception is thrown, then there is a logic error in test."));
            }
        }

        private readonly IElementContainer _browser;
        private readonly Constraint _elementConstraint;
        private readonly IReadOnlyCollection<TElement> _knownElements;
    }
}
