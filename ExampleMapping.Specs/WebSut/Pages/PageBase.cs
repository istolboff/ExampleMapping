using System;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class PageBase
    {
        protected PageBase(Browser browser, string pageUrl)
        {
            Browser = browser;
            _pageUrl = pageUrl;
            browser.GoTo(pageUrl);
        }

        public override string ToString()
        {
            return $"URL: {_pageUrl}{Environment.NewLine}Body:{Browser.Html}";
        }

        protected Browser Browser { get; }

        private readonly string _pageUrl;
    }
}
