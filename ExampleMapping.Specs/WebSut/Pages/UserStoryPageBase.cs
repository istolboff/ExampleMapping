using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class UserStoryPageBase : SubmittablePage
    {
        protected UserStoryPageBase(NavigableUrl navigableUrl)
            : base(navigableUrl)
        {
            _userStoryName = Url.Browser.TextField(Find.ByName("Name"));
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
