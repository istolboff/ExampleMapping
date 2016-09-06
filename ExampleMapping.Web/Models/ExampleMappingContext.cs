using Microsoft.EntityFrameworkCore;

namespace ExampleMapping.Web.Models
{
    public class ExampleMappingContext : DbContext
    {
        public ExampleMappingContext()
        {
        }

        public ExampleMappingContext(DbContextOptions<ExampleMappingContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<UserStory> UserStories { get; set; }
    }
}
