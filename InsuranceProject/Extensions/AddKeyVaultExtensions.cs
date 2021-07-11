using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
namespace InsuranceProject.Extensions
{
    public static class AddKeyVaultExtensions
    {
        public static IHostBuilder AddKeyVault(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration((Action<HostBuilderContext, IConfigurationBuilder>) ((ctx, builder) =>
            {
                string vault = builder.Build()["KeyVaultName"];
                if (string.IsNullOrEmpty(vault))
                    return;
                KeyVaultClient client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback.Invoke), Array.Empty<DelegatingHandler>());
                builder.AddAzureKeyVault(vault, client, (IKeyVaultSecretManager) new DefaultKeyVaultSecretManager());
            }));
            return host;
        }
    }
}