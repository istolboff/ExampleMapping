using System;
using System.Diagnostics.Contracts;
using System.IO;
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
        public static WebApplicationUnderTest ApplicationUnderTest { get; private set; }

        [BeforeTestRun]
        public static void SetupTestRun()
        {
            MakeCodeContractsToReportFailureViaExceptionsRatherThanViaMessageBoxes();
            WebApplicationDataRepository.EraseFromDisk();
            ApplicationUnderTest = new WebApplicationUnderTest(new IE(true), GetFreePort());
            IisExpress.RunWebProjectUnderTest(ApplicationUnderTest.PortNumber);
        }

        [BeforeScenario]
        public static void SetupScenario()
        {
            if (!File.Exists(WebProjectPathes.SqliteDatabaseFilePath))
            {
                return;
            }

            using (var applicationDataRepository = new WebApplicationDataRepository(WebProjectPathes.SqliteDatabaseFilePath))
            {
                applicationDataRepository.ClearEverything();
            }
        }

        [AfterTestRun]
        public static void TeardownTestRun()
        {
            ApplicationUnderTest.Dispose();
            IisExpress.Stop();
        }

        private static void MakeCodeContractsToReportFailureViaExceptionsRatherThanViaMessageBoxes()
        {
            Contract.ContractFailed += (_, contractFailedEventArgs) =>
            {
                contractFailedEventArgs.SetHandled();

                throw new InvalidOperationException(
                    $"Failed Code Contracts condition: {contractFailedEventArgs.Condition}. {contractFailedEventArgs.Message}",
                    contractFailedEventArgs.OriginalException);
            };
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
