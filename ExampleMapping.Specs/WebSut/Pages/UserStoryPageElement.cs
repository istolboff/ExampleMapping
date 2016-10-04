using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class UserStoryPageElement
    {
        public UserStoryPageElement(string userStoryName, Link editUserStoryLink, Browser browser)
        {
            UserStoryName = userStoryName;
            _editUserStoryLink = editUserStoryLink;
            _browser = browser;
        }

        public string UserStoryName { get; }

        public CreateEditUserStoryPage Edit()
        {
            return new CreateEditUserStoryPage(_browser, _editUserStoryLink);
        }

        private readonly Link _editUserStoryLink;
        private readonly Browser _browser;
    }
}
