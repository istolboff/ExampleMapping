using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;
using ExampleMapping.Specs.Miscellaneous;
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
            _addnewExampleLink = Browser.Link(Find.ById("AddNewExample"));
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
            var newRuleFinder = new NewlyCreatedElementFinder<TextField>(Browser, RuleTextElementIdRegex);
            _addNewRuleLink.Click();
            newRuleFinder.Result.TypeText(ruleText);
        }

        public void DeleteRule(string ruleText)
        {
            FindRuleElementsGroup(ruleText).DeleteButton.Click();
        }

        public void AddExample(string ruleText, string exampleText)
        {
            var newExampleFinder = new NewlyCreatedElementFinder<TextField>(Browser, ExampleTextElementRegex);
            var ruleElementsGroup = FindRuleElementsGroup(ruleText);
            _addnewExampleLink.Drag().DropTo(ruleElementsGroup.RuleText);
            newExampleFinder.Result.TypeText(exampleText);
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

        private RuleElementsGroup FindRuleElementsGroup(string ruleText)
        {
            return RuleElementsGroups.Single(elementsGroup => elementsGroup.RuleText.Text == ruleText);
        }

        private readonly TextField _userStoryName;
        private readonly Link _addNewRuleLink;
        private readonly Link _addnewExampleLink;

        private static readonly Regex RuleTextElementIdRegex = new Regex(@"^Rules\[\d+\]\.Name$", RegexOptions.Compiled);
        private static readonly Regex ExampleTextElementRegex = new Regex(@"^Rules\[\d+\]\.Examples\[\d+\]\.Name$", RegexOptions.Compiled);
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
