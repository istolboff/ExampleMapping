namespace ExampleMapping.Specs.WebSut.Pages
{
    internal sealed class UserStoryPageElement
    {
        public UserStoryPageElement(string userStoryName, NavigableUrl editUserStoryUrl)
        {
            UserStoryName = userStoryName;
            _editUserStoryUrl = editUserStoryUrl;
        }

        public string UserStoryName { get; }

        public EditUserStoryPage Edit()
        {
            return new EditUserStoryPage(_editUserStoryUrl);
        }

        private readonly NavigableUrl _editUserStoryUrl;
    }
}
