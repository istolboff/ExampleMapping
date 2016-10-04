using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class CreateEditUserStoryPage : SubmittablePage
    {
        public CreateEditUserStoryPage(Browser browser, string webProjectUrl)
            : base(browser, webProjectUrl + "/Create")
        {
            _userStoryName = browser.TextField(Find.ByName("Name"));
        }

        public CreateEditUserStoryPage(Browser browser, Link link) 
            : base(browser, link)
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
