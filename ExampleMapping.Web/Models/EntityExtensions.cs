using System.Linq;
using System.Collections.Generic;
using ExampleMapping.Web.Miscellaneous;

namespace ExampleMapping.Web.Models
{
    internal static class EntityExtensions
    {
        public static bool IsMarkedForDeletion<TEntity>(this TEntity @this) where TEntity : IEntity
        {
            return @this.Id < 0;
        }

        public static void RemoveEntitiesMarkedForDeletion<TEntity>(this ICollection<TEntity> @this)
            where TEntity : IEntity
        {
            @this.RemoveIf(IsMarkedForDeletion);
        }

        public static long[] GetIdsOfEntitiesMarkedForDeletion<TEntity>(this IEnumerable<TEntity> @this)
            where TEntity : IEntity
        {
            return @this.Where(entity => entity.IsMarkedForDeletion()).Select(entity => -entity.Id).ToArray();
        }
    }
}
