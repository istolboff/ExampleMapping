using System;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class PageBase
    {
        protected PageBase(NavigableUrl url)
        {
            Url = url;
            url.Navigate();
        }

        public override string ToString()
        {
            return $"URL: {Url}{Environment.NewLine}Body:{Browser.Html}";
        }

        protected NavigableUrl Url { get; }

        protected Browser Browser => Url.Browser;
    }
}
