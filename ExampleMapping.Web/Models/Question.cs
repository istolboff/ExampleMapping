namespace ExampleMapping.Web.Models
{
    public class Question
    {
        public long QuestionId { get; set; }

        public string Name { get; set; }

        public UserStory UserStory { get; set; }
    }
}
