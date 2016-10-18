namespace ExampleMapping.Web.Models
{
    public class Rule
    {
        public ulong RuleId { get; set; }

        public string Name { get; set; }

        public UserStory UserStory { get; set; }
    }
}
