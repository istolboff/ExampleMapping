namespace ExampleMapping.Web.Models
{
    public class Question : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
