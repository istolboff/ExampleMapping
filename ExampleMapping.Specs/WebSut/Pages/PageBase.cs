using System;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class PageBase
    {
        protected PageBase(NavigableUrl url)
        {
            Url = url.Uri;
            Browser = url.Browser;
            url.Navigate();
        }

        public override string ToString()
        {
            return $"URL: {Url}{Environment.NewLine}Body:{Browser.Html}";
        }

        protected Uri Url { get; }

        protected Browser Browser { get; }
    }
}
