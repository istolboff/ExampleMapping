using System;
using System.Linq;
using TechTalk.SpecFlow;
using ExampleMapping.Specs.WebSut.Pages;
using ExampleMapping.Specs.SpecFlow;
using ExampleMapping.Web.Models;
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
        public static void AddedNewRule(string ruleText)
        {
            CurrentUserStoryPage.AddRule(ruleText);
        }

        [Given(@"the User Story '(.*)' has the following Rules")]
        public static void UserStoryHasRules(string storyName, Table rulesText)
        {
            StartedToCreateUserStory(storyName);
            foreach (var ruleRow in rulesText.Rows)
            {
                AddedNewRule(ruleRow["Rule Text"]);
            }
            CompleteEditingTheUserStory();
        }

        [Given(@"I started to edit the User Story '(.*)'")]
        public static void StartedToEditUserStory(string storyName)
        {
            CurrentUserStoryPage = LoadUserStory(storyName);
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
        public static void UserStoryShouldHaveTheFollowingContent(string storyName, Table expectedStoryTabularContent)
        {
            var page = LoadUserStory(storyName);
            var storyContent = page.GetStoryContent();
            var matchingResult = expectedStoryTabularContent.Rows.Match(
                storyContent.Rules.Select(rule => new { RuleText = rule.Name }));
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
    }
}
