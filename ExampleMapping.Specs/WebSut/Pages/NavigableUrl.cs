using System;
using System.Diagnostics.Contracts;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class NavigableUrl
    {
        public NavigableUrl(Browser browser, Uri url)
        {
            Browser = browser;
            _url = url;
        }

        public NavigableUrl(Browser browser, Link link)
        {
            Browser = browser;
            _link = link;
        }

        public Browser Browser { get; }

        public void Navigate()
        {
            if (_url != null)
            {
                Browser.GoTo(_url);
            }
            else
            {
                _link.Click();
            }
        }

        public override string ToString()
        {
            return _url != null ? _url.ToString() : _link.Url;
        }

        public static NavigableUrl operator +(NavigableUrl url, string urlSuffix)
        {
            Contract.Assume(url._url != null);
            return new NavigableUrl(url.Browser, new Uri(url._url, url._url.AbsolutePath + urlSuffix));
        }

        private readonly Uri _url;
        private readonly Link _link;
    }
}
