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

builder.Services.AddTransient<CookieHandler>();
builder.Services.AddTransient(typeof(UniPassClientLogger<>));

// builder.Services.AddScoped(sp =>
//     new HttpClient { BaseAddress = new Uri("https://localhost:7094") });

builder.Services.AddHttpClient(
        AppData.AppName, 
        opt => opt.BaseAddress = new Uri("https://localhost:5002/api"))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<ClipboardService>();

builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<TeamService>();
builder.Services.AddTransient<KeyService>();
builder.Services.AddTransient<FolderService>();
builder.Services.AddScoped<ApplicationAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApplicationAuthenticationStateProvider>());

await builder.Build().RunAsync();