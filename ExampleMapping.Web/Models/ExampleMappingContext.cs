using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExampleMapping.Web.Models
{
    public class ExampleMappingContext : DbContext
    {
        public ExampleMappingContext()
        {
        }

        public ExampleMappingContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<UserStory> UserStories { get; set; }

        public DbSet<Rule> Rules { get; set; }

        public Task<UserStory> FindUserStoryById(long userStoryId)
        {
            return UserStories
                        .Include(userStory => userStory.Rules)
                        .SingleOrDefaultAsync(userStory => userStory.UserStoryId == userStoryId);
        }
    }
}
