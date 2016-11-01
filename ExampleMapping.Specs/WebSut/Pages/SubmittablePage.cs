using System.Diagnostics.Contracts;
using WatiN.Core;
using ExampleMapping.Specs.WebSut.WatinExtensions;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class SubmittablePage : PageBase
    {
        protected SubmittablePage(NavigableUrl url)
            : base(url)
        {
            _submitButton = url.Browser.Button(Find.ById("Submit"));
            Contract.Assume(_submitButton != null);
        }

        public void Submit()
        {
            _submitButton.Click();
            Browser.WaitForComplete();
            Browser.CheckRenderedPageForServerErrors();
        }

        private readonly Button _submitButton;
    }
}
