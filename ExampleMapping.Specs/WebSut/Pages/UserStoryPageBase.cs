﻿using System.Collections.Generic;
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
            _addNewQuestionLink = Browser.Link(Find.ById("AddNewQuestion"));
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
            FindRuleElementsGroup(ruleText).Delete();
        }

        public void AddExampleToRule(string ruleText, string exampleText)
        {
            var newExampleFinder = new NewlyCreatedElementFinder<TextField>(Browser, ExampleTextElementConstraint);
            var ruleElementsGroup = FindRuleElementsGroup(ruleText);
            Browser.Drag(_addnewExampleLink).DropTo(ruleElementsGroup.RuleText);
            newExampleFinder.Result.EnterText(exampleText);
        }

        public void DeleteExampleFromRule(string ruleText, string exampleText)
        {
            FindRuleElementsGroup(ruleText).FindExampleElementsGroup(exampleText).Delete();
        }

        public void AddQuestion(string questionText)
        {
            var newQuestionFinder = new NewlyCreatedElementFinder<TextField>(Browser, QuestionTextElementConstraint);
            _addNewQuestionLink.Click();
            newQuestionFinder.Result.EnterText(questionText);
        }

        public void DeleteQuestion(string questionText)
        {
            FindQuestionElementGroup(questionText).Delete();
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
                                    }).ToList(),
                    Questions = GetQuestionElementsGroups()
                                .OrderBy(elementsGroup => elementsGroup.QuestionText.Name)
                                .Select(elementsGroup => new Question { Name = elementsGroup.QuestionText.Text })
                                .ToList()
                };
        }

        private IEnumerable<RuleElementsGroup> GetRuleElementsGroups()
        {
            return Browser
                .Elements<Div>(Find.ByClass("Rule ElementsGroup"))
                .Select(ruleGroupDiv => new RuleElementsGroup(ruleGroupDiv));
        }

        private RuleElementsGroup FindRuleElementsGroup(string ruleText)
        {
            return GetRuleElementsGroups().Single(elementsGroup => elementsGroup.RuleText.Text == ruleText);
        }

        private IEnumerable<QuestionElementsGroup> GetQuestionElementsGroups()
        {
            return Browser
                .Elements<Div>(Find.ByClass("Question"))
                .Select(questionGroupDiv => new QuestionElementsGroup(questionGroupDiv));
        }

        private QuestionElementsGroup FindQuestionElementGroup(string questionText)
        {
            return GetQuestionElementsGroups().Single(elementsGroup => elementsGroup.QuestionText.Text == questionText);
        }

        private readonly TextField _userStoryName;
        private readonly Link _addNewRuleLink;
        private readonly Link _addnewExampleLink;
        private readonly Link _addNewQuestionLink;

        private static readonly Constraint RuleTextElementConstraint = Find.ByClass("Rule Wording");
        private static readonly Constraint ExampleTextElementConstraint = Find.ByClass("Example Wording");
        private static readonly Constraint QuestionTextElementConstraint = Find.ByClass("Question Wording");

        private class RuleElementsGroup
        {
            public RuleElementsGroup(IElementContainer ruleGroupDiv)
            {
                _ruleGroupDiv = ruleGroupDiv;
                RuleText = ruleGroupDiv.Elements<TextField>(RuleTextElementConstraint).Single();
                _deleteButton = ruleGroupDiv.Elements<Button>(Find.ByClass("deleteRule")).Single();
            }

            public TextField RuleText { get; }

            public void Delete()
            {
                _deleteButton.Click();
            }

            public IEnumerable<ExampleElementsGroup> GetExampleElementsGroups()
            {
                return _ruleGroupDiv.Elements<Div>(Find.ByClass("Example")).Select(divElement => new ExampleElementsGroup(divElement));
            }

            public ExampleElementsGroup FindExampleElementsGroup(string exampleText)
            {
                return GetExampleElementsGroups().Single(exampleGroup => exampleGroup.ExampleText.Text == exampleText);
            }

            private readonly IElementContainer _ruleGroupDiv;
            private readonly Button _deleteButton;
        }

        private class ExampleElementsGroup
        {
            public ExampleElementsGroup(IElementContainer divElement)
            {
                ExampleText = divElement.Elements<TextField>(ExampleTextElementConstraint).Single();
                _deleteButton = divElement.Elements<Button>(Find.ByClass("deleteExample")).Single();
            }

            public TextField ExampleText { get; }

            public void Delete()
            {
                _deleteButton.Click();
            }

            private readonly Button _deleteButton;
        }

        private class QuestionElementsGroup
        {
            public QuestionElementsGroup(IElementContainer divElement)
            {
                QuestionText = divElement.Elements<TextField>(QuestionTextElementConstraint).Single();
                _deleteButton = divElement.Elements<Button>(Find.ByClass("deleteQuestion")).Single();
            }

            public TextField QuestionText { get; }

            public void Delete()
            {
                _deleteButton.Click();
            }

            private readonly Button _deleteButton;
        }
    }
}