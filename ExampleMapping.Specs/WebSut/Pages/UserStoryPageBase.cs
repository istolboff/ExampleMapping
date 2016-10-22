using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ExampleMapping.Specs.Miscellaneous;
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

        public void DeleteRule(string ruleText)
        {
            var ruleElementsGroup = RuleElementsGroups.Single(elementsGroup => elementsGroup.RuleText.Text == ruleText);
            ruleElementsGroup.DeleteButton.Click();
        }

        public UserStory GetStoryContent()
        {
            return
                new UserStory
                {
                    Name = UserStoryName,
                    Rules = RuleElementsGroups.OrderBy(elementsGroup => elementsGroup.RuleText.Name).Select(elementsGroup => new Rule { Name = elementsGroup.RuleText.Text }).ToList()
                };
        }

        private IReadOnlyCollection<RuleElementsGroup> RuleElementsGroups
        {
            get
            {
                return
                    Browser.Elements<TextField>(RuleTextElementIdRegex)
                        .Zip(
                            Browser.Elements<Button>(DeleteRuleButtonIdRegex),
                            (ruleText, deleteButton) => new RuleElementsGroup(ruleText, deleteButton))
                        .AsImmutable();
            }
        }

        private readonly TextField _userStoryName;
        private readonly Link _addNewRuleLink;

        private static readonly Regex RuleTextElementIdRegex = new Regex(@"^Rules\[\d+\]\.Name$", RegexOptions.Compiled);
        private static readonly Regex DeleteRuleButtonIdRegex = new Regex(@"^DeleteRule_\d+$", RegexOptions.Compiled);

        private class RuleElementsGroup
        {
            public RuleElementsGroup(TextField ruleText, Button deleteButton)
            {
                RuleText = ruleText;
                DeleteButton = deleteButton;
            }

            public TextField RuleText { get; }

            public Button DeleteButton { get; }
        }
    }
}
