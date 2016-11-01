using System.Collections.Generic;

namespace ExampleMapping.Web.Models
{
    public class Rule
    {
        public long RuleId { get; set; }

        public string Name { get; set; }

        public UserStory UserStory { get; set; }

        public ICollection<Example> Examples { get; set; }
    }
}
