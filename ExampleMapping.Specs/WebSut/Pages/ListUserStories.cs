using System.Linq;
using ExampleMapping.Specs.Miscellaneous;
using WatiN.Core;

namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class ListUserStories : PageBase
    {
        public ListUserStories(NavigableUrl webProjectUrl)
            : base(webProjectUrl)
        {
        }

        public VerboseIndexer<string, UserStoryPageElement> UserStories
        {
            get
            {
                return
                    Browser.ElementsOfType<Div>()
                        .Where(element => element.Id == "StoryName")
                        .ToDictionary(element => element.Text, element => new UserStoryPageElement(element.Text, new NavigableUrl(Browser, (Link)element.Elements.Single())))
                        .WithVerboseIndexing("User Stories");
            }
        }
    }
}
