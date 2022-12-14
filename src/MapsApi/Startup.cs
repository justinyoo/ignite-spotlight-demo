using System;

using IgniteSpotlight.MapsApi.Configs;
using IgniteSpotlight.MapsApi.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly: FunctionsStartup(typeof(IgniteSpotlight.MapsApi.Startup))]
namespace IgniteSpotlight.MapsApi
{
    /// <summary>
    /// This represents the entity used for startup bootstrapping.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc/>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureAppSettings(builder.Services);
            ConfigureClients(builder.Services);
            ConfigureMapServices(builder.Services);
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            var settings = services.BuildServiceProvider()
                                   .GetService<IConfiguration>()
                                   .Get<MapsSettings>(MapsSettings.Name);
            services.AddSingleton(settings);

            var options = new DefaultOpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info = new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "Naver Map API Wrapper",
                    Description = "This is the facade API for Naver Maps."
                }
            };

            /* ⬇️⬇️⬇️ for GH Codespaces ⬇️⬇️⬇️ */
            var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
            if (codespaces)
            {
                options.IncludeRequestingHostName = false;
            }
            /* ⬆️⬆️⬆️ for GH Codespaces ⬆️⬆️⬆️ */

            services.AddSingleton<IOpenApiConfigurationOptions>(options);
        }

        private static void ConfigureClients(IServiceCollection services)
        {
            services.AddHttpClient("naver");
        }

        private static void ConfigureMapServices(IServiceCollection services)
        {
            services.AddSingleton<IMapServiceFactory, MapServiceFactory>();
        }
    }
}