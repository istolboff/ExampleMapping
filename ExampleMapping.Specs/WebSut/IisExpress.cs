using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;

namespace ExampleMapping.Specs.WebSut
{
    internal static class IisExpress
    {
        public static void RunWebProjectUnderTest(int port)
        {
            var startInfo = new ProcessStartInfo
                            {
                                WindowStyle = ProcessWindowStyle.Normal,
                                ErrorDialog = false,
                                LoadUserProfile = true,
                                CreateNoWindow = false,
                                UseShellExecute = false,
                                Arguments = $"/path:\"{WebProjectPathes.RootPath}\" /port:{port} /systray:false",
                                FileName = FullIisExpressExePath
            };
            
            startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", "dotnet");
            startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", WebProjectPathes.SiteDllPath);

            Trace.WriteLine($"Starting IIS Express: \"{startInfo.FileName}\" {startInfo.Arguments}  // %LAUNCHER_PATH%=dotnet; %LAUNCHER_ARGS%=\"{WebProjectPathes.SiteDllPath}\"");
            _iisExpressProcess = Process.Start(startInfo);
        }

        public static void Stop()
        {
            _iisExpressProcess.Kill();
        }

        private static string FullIisExpressExePath
        {
            get
            {
                var programFilesPath = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");
                Contract.Assume(!string.IsNullOrEmpty(programFilesPath));
                return string.Format(Path.Combine(programFilesPath, "IIS Express\\IisExpress.exe"));
            }
        }

        private static Process _iisExpressProcess;
    }
}
