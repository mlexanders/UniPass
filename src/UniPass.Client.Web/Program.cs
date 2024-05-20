using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using UniPass.Client;
using UniPass.Client.Services;
using UniPass.Client.Services.Api;
using UniPass.Domain.Application;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// !!! вот это
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddTransient(typeof(UniPassClientLogger<>));

// set base address for default host
    builder.Services.AddScoped(sp =>
        new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7094") });

// configure client for auth interactions
    builder.Services.AddHttpClient(
            AppData.AppName,//(Auth before)
            opt => opt.BaseAddress = new Uri("https://localhost:5002/api"))
        .AddHttpMessageHandler<CookieHandler>();


builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<TeamService>();
// builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();
builder.Services.AddScoped<ApplicationAuthenticationStateProvider>();


await builder.Build().RunAsync();