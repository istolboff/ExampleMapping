using System;
using WatiN.Core;
using ExampleMapping.Specs.WebSut.WatinExtensions;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class PageBase
    {
        protected PageBase(NavigableUrl url)
        {
            Url = url.Uri;
            Browser = url.Browser;
            url.Navigate();
            Browser.CheckRenderedPageForServerErrors();
        }

        public override string ToString()
        {
            return $"URL: {Url}{Environment.NewLine}Body:{Browser.Html}";
        }

        protected Uri Url { get; }

        protected Browser Browser { get; }
    }
}
