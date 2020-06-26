using Microsoft.Extensions.Configuration;
using System.IO;

namespace Core.Api.ConfigurationManager
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; }
        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}