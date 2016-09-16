using System.Diagnostics.Contracts;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class PageBase
    {
        protected PageBase(IWebDriver webDriver, string pageUrl)
        {
            Contract.Requires(webDriver != null);

            WebDriver = webDriver;
            webDriver.Navigate().GoToUrl(pageUrl);
            PageFactory.InitElements(webDriver, this);
        }

        protected IWebDriver WebDriver { get; }

        public void Submit()
        {
            throw new System.NotImplementedException();
        }
    }
}
