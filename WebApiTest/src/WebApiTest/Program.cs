using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace WebApiTest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "WebApi Test";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss.fff}] - {Level} - {Message}{NewLine}{Exception}")
                .CreateBootstrapLogger();

            Log.Information("Starting up!");
            Log.Information("Start Time is {Now}", DateTime.Now);

            try
            {
                CreateHostBuilder(args).Build().Run();

                Log.Information("Stopped cleanly");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}