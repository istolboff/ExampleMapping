using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class CreateUserStoryPage : SubmittablePage
    {
        public CreateUserStoryPage(Browser browser, string webProjectUrl)
            : base(browser, webProjectUrl + "/Create")
        {
            _userStoryName = browser.TextField(Find.ByName("Name"));
        }

        public string UserStoryName
        {
            set
            {
                _userStoryName.TypeText(value);
            }
        }

        private readonly TextField _userStoryName;
    }
}
