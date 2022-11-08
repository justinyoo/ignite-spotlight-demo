using System;

using FluentValidation;

using IgniteSpotlight.SmsCommon.Configurations;
using IgniteSpotlight.SmsFacadeApi.Configurations;
using IgniteSpotlight.SmsFacadeApi.Models;
using IgniteSpotlight.SmsFacadeApi.Validators;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly: FunctionsStartup(typeof(IgniteSpotlight.SmsFacadeApi.Startup))]
namespace IgniteSpotlight.SmsFacadeApi
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureAppSettings(builder.Services);
            ConfigureHttpClient(builder.Services);
            ConfigureValidators(builder.Services);
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            var toastSettings = services.BuildServiceProvider()
                                        .GetService<IConfiguration>()
                                        .Get<ToastSettings<SmsEndpointSettings>>(ToastSettings.Name);
            services.AddSingleton(toastSettings);

            var options = new DefaultOpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info = new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "NHN Cloud SMS API",
                    Description = "This is an API for sending SMS messages through the NHN Cloud SMS service."
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

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("messages");
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.AddSingleton<IRegexDateTimeWrapper, RegexDateTimeWrapper>();
            services.AddScoped<IValidator<SendMessagesRequestBody>, SendMessagesRequestBodyValidator>();
        }
    }
}