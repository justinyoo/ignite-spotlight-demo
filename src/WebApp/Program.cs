using IgniteSpotlight.WebApp;
using IgniteSpotlight.WebApp.Configs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// builder.Services.AddSingleton(sp => sp.GetService<IConfiguration>().GetSection(LinksSettings.Name).Get<LinksSettings>());
// builder.Services.AddSingleton(sp => sp.GetService<IConfiguration>().GetSection(EndpointsSettings.Name).Get<EndpointsSettings>());

builder.Services.AddGeolocationServices();

await builder.Build().RunAsync();
