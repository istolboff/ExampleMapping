using System.Net;
using System.Net.Sockets;
using TechTalk.SpecFlow;
using ExampleMapping.Specs.Miscellaneous;
using ExampleMapping.Specs.WebSut;
using ExampleMapping.Specs.WebSut.Pages;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;

namespace ExampleMapping.Specs.StepDefinitions
{
    [Binding]
    public static class UserStorySteps
    {
        [BeforeScenario]
        public static void SetupScenario()
        {
            Proxy proxy = new Proxy();
            proxy.HttpProxy = string.Format("127.0.0.1:9999");
            var service = PhantomJSDriverService.CreateDefaultService();
            service.ProxyType = "http";
            service.Proxy = proxy.HttpProxy;
            ApplicationUnderTest = new WebApplicationUnderTest(new PhantomJSDriver(service), GetFreePort());
            IisExpress.RunWebProjectUnderTest(ApplicationUnderTest.PortNumber);
        }

        [AfterScenario()]
        public static void TeardownScenario()
        {
            ApplicationUnderTest.Stop();
            IisExpress.Stop();
        }

        [Given(@"I have created a User Story with the name '(.*)'")]
        public static void GivenIHaveCreatedAUserStoryWithTheName(string storyName)
        {
            var page = ApplicationUnderTest.NavigateTo<CreateUserStoryPage>();
            page.UserStoryName = storyName;
            page.Submit();
        }

        [Then(@"the list of all stories should contain only the following items")]
        public static void ThenTheListOfAllStoriesShouldContainOnlyTheFollowingItems(Table expectedStories)
        {
            ScenarioContext.Current.Pending();
        }

        private static int GetFreePort()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                return ((IPEndPoint)sock.LocalEndPoint).Port;
            }
        }

        private static WebApplicationUnderTest ApplicationUnderTest;
    }
}
