using System.Linq;
using ExampleMapping.Specs.Miscellaneous;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class ListUserStories : PageBase
    {
        public ListUserStories(Browser browser, string webProjectUrl)
            : base(browser, webProjectUrl)
        {
        }

        public VerboseIndexer<string, UserStoryPageElement> UserStories
        {
            get
            {
                return
                    Browser.ElementsOfType<Div>()
                        .Where(element => element.Id == "StoryName")
                        .ToDictionary(element => element.Text, element => new UserStoryPageElement(element.Text, (Link)element.Elements.Single(), Browser))
                        .WithVerboseIndexing("User Stories");
            }
        }
    }
}
