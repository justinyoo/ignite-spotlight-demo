using System;

using FluentValidation;

using IgniteSpotlight.SmsCommon.Configurations;
using IgniteSpotlight.SmsVerifyFacadeApi.Configurations;
using IgniteSpotlight.SmsVerifyFacadeApi.Models;
using IgniteSpotlight.SmsVerifyFacadeApi.Validators;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly: FunctionsStartup(typeof(IgniteSpotlight.SmsVerifyFacadeApi.Startup))]
namespace IgniteSpotlight.SmsVerifyFacadeApi
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
                                        .Get<ToastSettings<SmsVerificationEndpointSettings>>(ToastSettings.Name);
            services.AddSingleton(toastSettings);

            var options = new DefaultOpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info = new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "NHN Cloud SMS Verification API",
                    Description = "This is an API for verifying SMS senders through the NHN Cloud SMS service."
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
            services.AddHttpClient("senders");
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<ListSendersRequestQueries>, ListSendersRequestQueryValidator>();
        }
    }
}