using System.Collections.Generic;

namespace ExampleMapping.Web.Models
{
    public class UserStory : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<Rule> Rules { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
