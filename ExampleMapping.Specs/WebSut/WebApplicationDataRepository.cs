using System;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using System.IO;

namespace ExampleMapping.Specs.WebSut
{
    internal class WebApplicationDataRepository : IDisposable
    {
        public WebApplicationDataRepository(string sqliteFilePath)
        {
            Contract.Requires(File.Exists(sqliteFilePath));

            _sqLiteConnection = new SQLiteConnection($"Data Source={sqliteFilePath}");
            _sqLiteConnection.Open();
        }

        public void ClearEverything()
        {
            foreach (var tableName in new[] { "UserStories", "Rules" })
            {
                ClearTable(tableName);
            }
        }

        public static void EraseFromDisk()
        {
            File.Delete(WebProjectPathes.SqliteDatabaseFilePath);
        }

        public void Dispose()
        {
            _sqLiteConnection.Dispose();
        }

        private void ClearTable(string tableName)
        {
            using (var sqLiteCommand = _sqLiteConnection.CreateCommand())
            {
                sqLiteCommand.CommandText = $"delete from {tableName}";
                sqLiteCommand.ExecuteNonQuery();
            }
        }

        private readonly SQLiteConnection _sqLiteConnection;
    }
}
