using System.Collections.Generic;
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

        public IReadOnlyCollection<string> UserStoryNames
        {
            get
            {
                return
                    Browser.ElementsOfType<Div>()
                        .Where(element => element.Id == "StoryName")
                        .Select(element => element.Text)
                        .AsImmutable();
            }
        }
    }
}
