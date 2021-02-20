using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WeatherApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Read settings to configure SeriLog
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                // As we are in static main we need to use the static "Log".
                // Not using ILogger just now as dependency injection not present here.
                Log.Information("Application Starting Up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                
                Log.Fatal(ex, "The application failed to start correctly.");
            }
            finally
            {
                // Any pending log messages will be pushed.
                Log.CloseAndFlush();
            }
  
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
