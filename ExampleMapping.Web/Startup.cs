using System.Diagnostics.Contracts;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ExampleMapping.Web.Models;

namespace ExampleMapping.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Contract.Assume(_sqliteDatabaseFile == null);
            _sqliteDatabaseFile = EnsureDatabaseCreated(env);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Contract.Assume(_sqliteDatabaseFile != null);
            services.AddDbContext<ExampleMappingContext>(options => options.UseSqlite(@"Data Source=" + _sqliteDatabaseFile.FullName));
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private FileInfo EnsureDatabaseCreated(IHostingEnvironment env)
        {
            var contentRootDirectory = new DirectoryInfo(env.ContentRootPath);
            var databaseDirectory = contentRootDirectory.CreateSubdirectory("DataBase");
            var result = new FileInfo(Path.Combine(databaseDirectory.FullName, "ExampleMapping.sqlite"));
            using (var db = new SelfCreatingExampleMappingContext(result))
            {
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }

            return result;
        }

        private readonly FileInfo _sqliteDatabaseFile;
    }
}
