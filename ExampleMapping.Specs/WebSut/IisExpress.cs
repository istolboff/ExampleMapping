using System;
using System.Diagnostics;
using System.IO;

namespace ExampleMapping.Specs.Miscellaneous
{
    internal static class IisExpress
    {
        public static void RunWebProjectUnderTest(int port)
        {
            var programFiles = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");

            var startInfo = new ProcessStartInfo
                            {
                                WindowStyle = ProcessWindowStyle.Normal,
                                ErrorDialog = false,
                                LoadUserProfile = true,
                                CreateNoWindow = false,
                                UseShellExecute = false,
                                Arguments = $"/path:\"{WebProjectUnderTestPath}\" /port:{port} /systray:false",
                                FileName = FullIisExpressExePath
            };
            
            startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", "dotnet");
            startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", SiteUnderTestDllPath);

            Trace.WriteLine($"Starting IIS Express: \"{startInfo.FileName}\" {startInfo.Arguments}  // %LAUNCHER_PATH%=dotnet; %LAUNCHER_ARGS%=\"{SiteUnderTestDllPath}\"");
            IisExpressProcess = Process.Start(startInfo);
        }

        public static void Stop()
        {
            IisExpressProcess.Kill();
        }

        private static string FullIisExpressExePath
        {
            get
            {
                var programFilesPath = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");
                return string.Format(Path.Combine(programFilesPath, "IIS Express\\IisExpress.exe"));
            }
        }

        private static string WebProjectUnderTestPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(ThisAssemblyFolderPath, @"..\..\" + WebProjectName));
            }
        }

        private static string SiteUnderTestDllPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(ThisAssemblyFolderPath, $"..\\{WebProjectName}\\bin\\{ConfigurationName}\\netcoreapp1.0\\{WebProjectName}.dll"));
            }
        }

        private const string WebProjectName = "ExampleMapping.Web";

        private static readonly string ThisAssemblyFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

#if DEBUG
        private const string ConfigurationName = "Debug";
#else
        private const string ConfigurationName = "Release";
#endif

        private static Process IisExpressProcess;
    }
}
