using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Beam.Client;
using Beam.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<BeamApiService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<BeamAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BeamAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<Beam.Animation.Javascript>();
await builder.Build().RunAsync();
