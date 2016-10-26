using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal static class ElementContainerExtensions
    {
        public static IEnumerable<TElement> Elements<TElement>(this IElementContainer @this, Constraint elementConstraint) where TElement : Element
        {
            return @this.Elements.Filter(elementConstraint).OfType<TElement>();
        }
    }
}
