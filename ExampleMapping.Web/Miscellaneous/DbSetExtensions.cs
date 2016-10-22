using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ExampleMapping.Web.Miscellaneous
{
    internal static class DbSetExtensions
    {
        public static void RemoveIf<TEntity>(this DbSet<TEntity> @this, Func<TEntity, bool> predicate) where TEntity : class
        {
            var entitiesToRemove = @this.Where(predicate).AsImmutable();
            @this.RemoveRange(entitiesToRemove);
        }
    }
}
