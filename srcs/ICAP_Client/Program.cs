using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ICAP_Client;
using ICAP_Client.RESTClients;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://510c0087-cfa7-41a0-8d34-9756d4d903a9/access_as_user");
});

builder.Services.AddScoped<AccountServiceClient>();
builder.Services.AddScoped<MarketServiceClient>();
builder.Services.AddScoped<RelationServiceClient>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
