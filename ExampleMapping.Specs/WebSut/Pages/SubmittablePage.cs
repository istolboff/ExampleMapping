using System;
using System.Diagnostics.Contracts;
using ExampleMapping.Specs.Miscellaneous;
using WatiN.Core;

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
            Verify.That(
                !Browser.Html.Contains("An unhandled exception occurred while processing the request"),
                () => Browser.Html);
        }

        private readonly Button _submitButton;
    }
}
