namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class CreateUserStoryPage : UserStoryPageBase
    {
        public CreateUserStoryPage(NavigableUrl navigableUrl)
            : base(navigableUrl + "/Create")
        {
        }
    }
}
