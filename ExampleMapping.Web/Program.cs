using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ExampleMapping.Web
{
    public class Program
    {
        public static void Main()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
