using System.IO;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ExampleMapping.Web.Models
{
    public class SelfCreatingExampleMappingContext : ExampleMappingContext
    {
        public SelfCreatingExampleMappingContext(FileInfo sqliteDatabaseFile)
        {
            Contract.Requires(sqliteDatabaseFile != null);

            _sqliteDatabaseFile = sqliteDatabaseFile;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + _sqliteDatabaseFile.FullName);
        }

        private readonly FileInfo _sqliteDatabaseFile;
    }
}
