using System.Diagnostics.Contracts;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class SubmittablePage : PageBase
    {
        protected SubmittablePage(Browser browser, string pageUrl)
            : base(browser, pageUrl)
        {
            _submitButton = browser.Button(Find.ById("Submit"));
            Contract.Assume(_submitButton != null);
        }

        protected SubmittablePage(Browser browser, Link link)
            : base(browser, link)
        {
            _submitButton = browser.Button(Find.ById("Submit"));
            Contract.Assume(_submitButton != null);
        }

        public void Submit()
        {
            _submitButton.Click();
        }

        private readonly Button _submitButton;
    }
}
