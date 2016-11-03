using System.Collections.Generic;

namespace ExampleMapping.Web.Models
{
    public class UserStory
    {
        public UserStory()
        {
            Rules = new List<Rule>();
        }

        public long UserStoryId { get; set; }

        public string Name { get; set; }

        public ICollection<Rule> Rules { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
