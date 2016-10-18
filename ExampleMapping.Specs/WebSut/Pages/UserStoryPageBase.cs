using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;
using ExampleMapping.Specs.WebSut.WatinExtensions;
using ExampleMapping.Web.Models;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal abstract class UserStoryPageBase : SubmittablePage
    {
        protected UserStoryPageBase(NavigableUrl navigableUrl)
            : base(navigableUrl)
        {
            _userStoryName = Browser.TextField(Find.ByName("Name"));
            _addNewRuleLink = Browser.Link(Find.ById("AddNewRule"));
        }

        public string UserStoryName
        {
            private get
            {
                return _userStoryName.Text;
            }

            set
            {
                _userStoryName.TypeText(value);
            }
        }

        public void AddRule(string ruleText)
        {
            var newlyCreatedElementFinder = new NewlyCreatedElementFinder<TextField>(Browser, RuleTextElementIdRegex);
            _addNewRuleLink.Click();
            newlyCreatedElementFinder.Result.TypeText(ruleText);
        }

        public UserStory GetStoryContent()
        {
            return
                new UserStory
                {
                    Name = UserStoryName,
                    Rules = Browser.Elements<TextField>(RuleTextElementIdRegex).OrderBy(textField => textField.Name).Select(textField => new Rule { Name = textField.Text }).ToList()
                };
        }

        private readonly TextField _userStoryName;
        private readonly Link _addNewRuleLink;

        private static readonly Regex RuleTextElementIdRegex = new Regex("^Rules\\[\\d+\\]\\.Name$", RegexOptions.Compiled);
    }
}
