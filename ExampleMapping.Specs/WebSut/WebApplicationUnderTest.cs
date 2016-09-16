using System;
using System.Diagnostics.Contracts;
using OpenQA.Selenium;
using ExampleMapping.Specs.WebSut.Pages;

namespace ExampleMapping.Specs.WebSut
{
    internal sealed class WebApplicationUnderTest
    {
        public WebApplicationUnderTest(IWebDriver webDriver, int portNumber)
        {
            Contract.Requires(webDriver != null);

            _webDriver = webDriver;
            PortNumber = portNumber;
        }

        public int PortNumber { get; }

        public TPage NavigateTo<TPage>() where TPage : PageBase
        {
            return Activator.CreateInstance(typeof(TPage), _webDriver) as TPage;
        }

        public void Stop()
        {
            _webDriver.Quit();
        }

        private readonly IWebDriver _webDriver;
    }
}
