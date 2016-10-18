using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.WatinExtensions
{
    internal static class ElementContainerExtensions
    {
        public static IEnumerable<TElement> Elements<TElement>(this IElementContainer @this, Regex elementIdRegex) where TElement : Element
        {
            return @this.Elements
                .OfType<TElement>()
                .Where(element => elementIdRegex.Match(element.IdOrName).Success);
        }
    }
}
