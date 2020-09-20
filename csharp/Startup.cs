using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(GottaBeGeek.Startup))]

namespace GottaBeGeek
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";
            var actualRoot = localRoot ?? azureRoot;
            var configBuilder = new ConfigurationBuilder()
                                .SetBasePath(actualRoot)
                                .AddEnvironmentVariables()
                                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

            var config = configBuilder.Build();
            var keyVaultUri = config.GetValue<string>("KeyVaultUri");
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            configBuilder.AddAzureKeyVault(keyVaultUri, keyVaultClient, new DefaultKeyVaultSecretManager());

            config = configBuilder.Build();
            builder.Services.Configure<TodoOptions>(config.GetSection(TodoOptions.Todos));
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));
            builder.Services.AddSingleton<ITodoManager, TodoManager>();
        }
    }
}