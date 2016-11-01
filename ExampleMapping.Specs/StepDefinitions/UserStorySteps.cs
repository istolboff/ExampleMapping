﻿using System;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using ExampleMapping.Specs.WebSut.Pages;
using ExampleMapping.Specs.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExampleMapping.Specs.StepDefinitions
{
    [Binding]
    public static class UserStorySteps
    {
        [Given(@"I have created a User Story with the name '(.*)'")]
        public static void CreatedAUserStoryWithTheName(string storyName)
        {
            var page = TestRun.ApplicationUnderTest.NavigateTo<CreateUserStoryPage>();
            page.UserStoryName = storyName;
            page.Submit();
        }

        [Given(@"I changed the name of the story from '(.*)' to '(.*)'")]
        public static void ChangedTheNameOfTheStoryTo(string oldStoryName, string newStoryName)
        {
            var editStoryPage = LoadUserStory(oldStoryName);
            editStoryPage.UserStoryName = newStoryName;
            editStoryPage.Submit();
        }

        [Given(@"I started to create a new User Story with the name '(.*)'")]
        public static void StartedToCreateUserStory(string storyName)
        {
            CurrentUserStoryPage = TestRun.ApplicationUnderTest.NavigateTo<CreateUserStoryPage>();
            CurrentUserStoryPage.UserStoryName = storyName;
        }

        [Given(@"I added a new Rule that says '(.*)'")]
        public static void AddNewRule(string ruleText)
        {
            AddNewRuleCore(ruleText, Enumerable.Empty<string>());
        }

        [Given(@"the User Story '(.*)' has the following Rules")]
        public static void UserStoryHasRules(string storyName, Table rulesText)
        {
            StartedToCreateUserStory(storyName);
            foreach (var ruleRow in rulesText.Rows)
            {
                var exampleTexts = ruleRow.ContainsKey("Rule Examples")
                    ? ruleRow["Rule Examples"].Trim(' ', '{', '}').Split(new[] { "},{", "}, {" }, StringSplitOptions.None)
                    : Enumerable.Empty<string>();

                AddNewRuleCore(ruleRow["Rule Text"], exampleTexts);
            }

            CompleteEditingTheUserStory();
        }

        [Given(@"I started to edit the User Story '(.*)'")]
        public static void StartedToEditUserStory(string storyName)
        {
            CurrentUserStoryPage = LoadUserStory(storyName);
        }

        [Given(@"I illustrated the Rule '(.*)' with the example '(.*)'")]
        public static void RuleIsIllustratedWithExample(string ruleText, string exampleText)
        {
            CurrentUserStoryPage.AddExample(ruleText, exampleText);
        }

        [When(@"I complete editing the User Story")]
        public static void CompleteEditingTheUserStory()
        {
            CurrentUserStoryPage.Submit();
        }

        [When(@"I delete the Rule '(.*)'")]
        public static void DeleteRule(string ruleText)
        {
            CurrentUserStoryPage.DeleteRule(ruleText);
        }

        [When(@"I delete the Example that says '(.*)' from the Rule '(.*)'")]
        public static void DeleteExampleFromRule(string exampleText, string ruleText)
        {
            CurrentUserStoryPage.DeleteExampleFromRule(exampleText: exampleText, ruleText: ruleText);
        }

        [Then(@"the list of all stories should contain only the following items")]
        public static void ListOfAllStoriesShouldContainOnlyTheFollowingItems(Table expectedStories)
        {
            var page = TestRun.ApplicationUnderTest.NavigateTo<ListUserStories>();
            var matchingResult = expectedStories.Rows.Match(page.UserStories.Select(userStory => new { UserStoryName = userStory.Key }));
            Assert.IsTrue(
                matchingResult,
                $"There are discepancies between expected and actual User Story names.{Environment.NewLine}{matchingResult}{Environment.NewLine}{page}");
        }

        [Then(@"the '(.*)' User Story should have the following Rules")]
        public static void UserStoryShouldHaveTheFollowingRules(string storyName, Table expectedStoryTabularContent)
        {
            var page = LoadUserStory(storyName);
            var storyContent = page.GetStoryContent();
            var matchingResult = expectedStoryTabularContent.Rows.Match(
                storyContent.Rules.Select(rule => 
                    new
                    {
                        RuleText = rule.Name,
                        RuleExamples = string.Join(", ", rule.Examples.Select(example => "{" + example.Name + "}"))
                    }));
            Assert.AreEqual(storyName, storyContent.Name);
            Assert.IsTrue(
                matchingResult,
                $"Expected and actual user story contents differ: {Environment.NewLine}{matchingResult}{Environment.NewLine}{page}");
        }

        private static UserStoryPageBase CurrentUserStoryPage
        {
            get
            {
                return ScenarioContext.Current.Get<UserStoryPageBase>();
            }

            set
            {
                ScenarioContext.Current.Set(value);
            }
        }

        private static EditUserStoryPage LoadUserStory(string storyName)
        {
            var page = TestRun.ApplicationUnderTest.NavigateTo<ListUserStories>();
            return page.UserStories[storyName].Edit();
        }

        private static void AddNewRuleCore(string ruleText, IEnumerable<string> exampleTexts)
        {
            CurrentUserStoryPage.AddRule(ruleText);
            foreach (var exampleText in exampleTexts)
            {
                CurrentUserStoryPage.AddExample(ruleText, exampleText);
            }
        }
    }
}
