namespace ExampleMapping.Web.Models
{
    public class Rule
    {
        public long RuleId { get; set; }

        public string Name { get; set; }

        public UserStory UserStory { get; set; }
    }
}
