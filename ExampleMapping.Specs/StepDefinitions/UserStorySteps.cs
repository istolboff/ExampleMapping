using System;
using System.Linq;
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
        public static void GivenIHaveCreatedAUserStoryWithTheName(string storyName)
        {
            var page = TestRun.ApplicationUnderTest.NavigateTo<CreateUserStoryPage>();
            page.UserStoryName = storyName;
            page.Submit();
        }

        [Given(@"I changed the name of the story from '(.*)' to '(.*)'")]
        public static void GivenIChangedTheNameOfTheStoryTo(string oldStoryName, string newStoryName)
        {
            var allStoriesPage = TestRun.ApplicationUnderTest.NavigateTo<ListUserStories>();
            var editStoryPage = allStoriesPage.UserStories[oldStoryName].Edit();
            editStoryPage.UserStoryName = newStoryName;
            editStoryPage.Submit();
        }

        [Then(@"the list of all stories should contain only the following items")]
        public static void ThenTheListOfAllStoriesShouldContainOnlyTheFollowingItems(Table expectedStories)
        {
            var page = TestRun.ApplicationUnderTest.NavigateTo<ListUserStories>();
            var matchingResult = expectedStories.Rows.Match(page.UserStories.Select(userStory => new { UserStoryName = userStory.Key }));
            Assert.IsTrue(
                matchingResult,
                $"There are discepancies between expected and actual User Story names.{Environment.NewLine}{matchingResult}{Environment.NewLine}{page}");
        }
    }
}
