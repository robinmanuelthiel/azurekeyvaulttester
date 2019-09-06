using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace KeyVaultTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = configBuilder.Build();

            // Configure Azure Key Vault Connection
            var uri = configuration["AzureKeyVaultUri"];
            var clientId = configuration["AzureKeyVaultClientId"];
            var clientSecret = configuration["AzureKeyVaultClientSecret"];

            // Check, if Client ID and Client Secret credentials for a Service Principal have been provided.
            // If so, use them to connect, otherwise let the connection be done automatically in the
            // background
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
                configBuilder.AddAzureKeyVault(uri, clientId, clientSecret);
            else
                configBuilder.AddAzureKeyVault(uri);

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configBuilder.Build())
                .UseStartup<Startup>();
        }
    }
}
