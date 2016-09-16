using ExampleMapping.Specs.Miscellaneous;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class CreateUserStoryPage : PageBase
    {
        public CreateUserStoryPage(IWebDriver webDriver, string webProjectUrl)
            : base(webDriver, webProjectUrl + "/Create")
        {
        }

        public string UserStoryName
        {
            set
            {
                _userStoryName.SendKeys(value);
            }
        }

        [FindsBy(How = How.Id, Using = "Name")]
        private readonly IWebElement _userStoryName = MakeCompilerHappy.InitialWebElementValue;
    }
}
