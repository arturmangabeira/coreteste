using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IntegradorIdea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string loggingFilePath = ConfigurationManager.ConfigurationManager.AppSettings["Logging:FilePath"];
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(loggingFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            try
            {
                Log.Information("Incializando a aplicacao");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Erro ao incializar a aplicação");
            }
            finally
            {
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
