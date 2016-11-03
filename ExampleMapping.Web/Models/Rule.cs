using System.Collections.Generic;

namespace ExampleMapping.Web.Models
{
    public class Rule
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<Example> Examples { get; set; }
    }
}
