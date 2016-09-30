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
            using (var sqLiteCommand = _sqLiteConnection.CreateCommand())
            {
                sqLiteCommand.CommandText = "delete from UserStories";
                sqLiteCommand.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _sqLiteConnection.Dispose();
        }

        private readonly SQLiteConnection _sqLiteConnection;
    }
}
