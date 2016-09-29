using System.IO;

namespace ExampleMapping.Specs.WebSut
{
    internal static class WebProjectPathes
    {
        public static string RootPath => Path.GetFullPath(Path.Combine(ThisAssemblyFolderPath, @"..\..\" + WebProjectName));

        public static string SiteDllPath => Path.GetFullPath(Path.Combine(ThisAssemblyFolderPath, $"..\\{WebProjectName}\\bin\\{ConfigurationName}\\netcoreapp1.0\\{WebProjectName}.dll"));

        public static string SqliteDatabaseFilePath => Path.Combine(RootPath, @"DataBase\ExampleMapping.sqlite");

        private const string WebProjectName = "ExampleMapping.Web";

        private static readonly string ThisAssemblyFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

#if DEBUG
        private const string ConfigurationName = "Debug";
#else
        private const string ConfigurationName = "Release";
#endif
    }
}
