using System.IO;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CacheServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
           
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Config)
                .ConfigureMetricsWithDefaults(
                    builder =>
                    {
                        //builder.Report.ToConsole();
                    })
                .UseMetricsWebTracking()
                .UseMetrics()
                .UseHealth()
                .UseStartup<Startup>();
        
        private static void Config(WebHostBuilderContext ctx, IConfigurationBuilder config)
        {            
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("hosting.json", true)
                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true,
                    reloadOnChange: true);

        }

    }
}
