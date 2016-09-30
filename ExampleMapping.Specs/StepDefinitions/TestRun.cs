using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;
using TechTalk.SpecFlow;
using ExampleMapping.Specs.WebSut;
using WatiN.Core;

namespace ExampleMapping.Specs.StepDefinitions
{
    [Binding]
    public static class TestRun
    {
        public static WebApplicationUnderTest ApplicationUnderTest;

        [BeforeTestRun]
        public static void SetupTestRun()
        {
            ApplicationUnderTest = new WebApplicationUnderTest(new IE(true), GetFreePort());
            IisExpress.RunWebProjectUnderTest(ApplicationUnderTest.PortNumber);
        }

        [BeforeScenario]
        public static void SetupScenario()
        {
            using (var sqLiteConnection = new SQLiteConnection($"Data Source={WebProjectPathes.SqliteDatabaseFilePath}"))
            {
                sqLiteConnection.Open();
                using (var sqLiteCommand = sqLiteConnection.CreateCommand())
                {
                    sqLiteCommand.CommandText = "delete from UserStories";
                    sqLiteCommand.ExecuteNonQuery();
                }
            }
        }

        [AfterScenario]
        public static void TeardownScenario()
        {
            ApplicationUnderTest.Dispose();
            IisExpress.Stop();
        }

        private static int GetFreePort()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                return ((IPEndPoint)sock.LocalEndPoint).Port;
            }
        }
    }
}
