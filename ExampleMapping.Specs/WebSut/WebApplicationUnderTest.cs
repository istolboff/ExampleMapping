using System;
using System.Diagnostics.Contracts;
using ExampleMapping.Specs.WebSut.Pages;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut
{
    public sealed class WebApplicationUnderTest : IDisposable
    {
        public WebApplicationUnderTest(Browser browser, int portNumber)
        {
            Contract.Requires(_browser != null);
            Contract.Requires(portNumber >= 0);

            _browser = browser;
            PortNumber = portNumber;
            _webProjectUrl = new Uri($"http://localhost:{portNumber}/UserStories");
        }

        public int PortNumber { get; }

        public TPage NavigateTo<TPage>() where TPage : class 
        {
            return Activator.CreateInstance(typeof(TPage), new NavigableUrl(_browser, _webProjectUrl)) as TPage;
        }

        public void Dispose()
        {
            _browser.Close();
            _browser.Dispose();
        }

        private readonly Browser _browser;
        private readonly Uri _webProjectUrl;
    }
}
