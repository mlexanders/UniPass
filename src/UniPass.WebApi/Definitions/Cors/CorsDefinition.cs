using System.Text.Json.Serialization;
using Calabonga.AspNetCore.AppDefinitions;
using UniPass.Infrastructure;

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
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AppData.PolicyCorsName, policyBuilder =>
            {
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();

                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                policyBuilder.SetIsOriginAllowed(host => true);
                policyBuilder.AllowCredentials();
            });
        });
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseCors(AppData.PolicyCorsName);
    }
}