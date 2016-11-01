using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Constraints;
using ExampleMapping.Web.Models;
using ExampleMapping.Specs.WebSut.WatinExtensions;

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
                _userStoryName.EnterText(value);
            }
        }

        public void AddRule(string ruleText)
        {
            var newRuleFinder = new NewlyCreatedElementFinder<TextField>(Browser, RuleTextElementConstraint);
            _addNewRuleLink.Click();
            newRuleFinder.Result.EnterText(ruleText);
        }

        public void DeleteRule(string ruleText)
        {
            FindRuleElementsGroup(ruleText).DeleteButton.Click();
        }

        public void AddExample(string ruleText, string exampleText)
        {
            var newExampleFinder = new NewlyCreatedElementFinder<TextField>(Browser, ExampleTextElementConstraint);
            var ruleElementsGroup = FindRuleElementsGroup(ruleText);
            Browser.Drag(_addnewExampleLink).DropTo(ruleElementsGroup.RuleText);
            newExampleFinder.Result.EnterText(exampleText);
        }

        public void DeleteExampleFromRule(string ruleText, string exampleText)
        {
            FindRuleElementsGroup(ruleText).FindExampleElementsGroup(exampleText).DeleteButton.Click();
        }

        public UserStory GetStoryContent()
        {
            return
                new UserStory
                {
                    Name = UserStoryName,
                    Rules = GetRuleElementsGroups()
                                .OrderBy(elementsGroup => elementsGroup.RuleText.Name)
                                .Select(elementsGroup => 
                                    new Rule
                                    {
                                        Name = elementsGroup.RuleText.Text,
                                        Examples = elementsGroup
                                                        .GetExampleElementsGroups()
                                                        .Select(exampleGroup => new Example { Name = exampleGroup.ExampleText.Text })
                                                        .ToList()
                                    }).ToList()
                };
        }

        private IEnumerable<RuleElementsGroup> GetRuleElementsGroups()
        {
            return Browser
                .Elements<Div>(RuleElementsGroupConstraint)
                .Select(ruleGroupDiv => new RuleElementsGroup(ruleGroupDiv));
        }

        private RuleElementsGroup FindRuleElementsGroup(string ruleText)
        {
            return GetRuleElementsGroups().Single(elementsGroup => elementsGroup.RuleText.Text == ruleText);
        }

        private readonly TextField _userStoryName;
        private readonly Link _addNewRuleLink;
        private readonly Link _addnewExampleLink;

        private static readonly Constraint RuleElementsGroupConstraint = Find.ByClass("ruleElementsGroup");
        private static readonly Constraint ExampleElementsGroupConstraint = Find.ByClass("exampleElementsGroup");
        private static readonly Constraint RuleTextElementConstraint = Find.ByClass("ruleWording");
        private static readonly Constraint ExampleTextElementConstraint = Find.ByClass("exampleWording");
        private static readonly Constraint DeleteRuleButtonConstraint = Find.ByClass("deleteRule");
        private static readonly Constraint DeleteExampleButtonConstraint = Find.ByClass("deleteExample");

        private class RuleElementsGroup
        {
            public RuleElementsGroup(Div ruleGroupDiv)
            {
                _ruleGroupDiv = ruleGroupDiv;
                RuleText = ruleGroupDiv.Elements<TextField>(RuleTextElementConstraint).Single();
                DeleteButton = ruleGroupDiv.Elements<Button>(DeleteRuleButtonConstraint).Single();
            }

            public TextField RuleText { get; }

            public Button DeleteButton { get; }

            public IEnumerable<ExampleElementsGroup> GetExampleElementsGroups()
            {
                return _ruleGroupDiv.Elements<Div>(ExampleElementsGroupConstraint).Select(divElement => new ExampleElementsGroup(divElement));
            }

            public ExampleElementsGroup FindExampleElementsGroup(string exampleText)
            {
                return GetExampleElementsGroups().Single(exampleGroup => exampleGroup.ExampleText.Text == exampleText);
            }

            private readonly Div _ruleGroupDiv;
        }

        private class ExampleElementsGroup
        {
            public ExampleElementsGroup(Div divElement)
            {
                _divElement = divElement;
                ExampleText = divElement.Elements<TextField>(ExampleTextElementConstraint).Single();
                DeleteButton = divElement.Elements<Button>(DeleteExampleButtonConstraint).Single();
            }

            public TextField ExampleText { get; }

            public Button DeleteButton { get; }

            private readonly Div _divElement;
        }
    }
}
