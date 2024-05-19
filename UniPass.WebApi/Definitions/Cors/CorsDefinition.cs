using System.Text.Json.Serialization;
using Calabonga.AspNetCore.AppDefinitions;
using UniPass.Domain.Application;

namespace UniPass.WebApi.Definitions.Cors;

/// <summary>
///     Cors configurations
/// </summary>
public class CorsDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current application
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AppData.PolicyCorsName, policyBuilder =>
            {
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                if (origins is not { Length: > 0 }) return;

                if (origins.Contains("*"))
                {
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.SetIsOriginAllowed(host => true);
                    policyBuilder.AllowCredentials();
                }
                else
                {
                    foreach (var origin in origins) policyBuilder.WithOrigins(origin);
                }
            });
        });
        
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseCors(AppData.PolicyCorsName);
    }
}