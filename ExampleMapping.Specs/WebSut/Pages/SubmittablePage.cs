using System.Diagnostics.Contracts;
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
        }

        private readonly Button _submitButton;
    }
}
