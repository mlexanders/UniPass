using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using UniPass.Domain.Application;

namespace UniPass.WebApi.Definitions.Swagger;

public class SwaggerDefinition : AppDefinition
{
    // ATTENTION!
    // If you use are git repository then you can uncomment line with "ThisAssembly" below for versioning by GIT possibilities.
    // Otherwise, you can change versions of your API by manually.
    // If you are not going to use git-versioning, do not forget install package "GitInfo" 
    // private const string AppVersion = $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";
    public const string AppVersion = "1.0.0";

    private const string SwaggerConfig = "/swagger/v1/swagger.json";

    public override void ConfigureApplication(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI(settings =>
        {
            settings.SwaggerEndpoint(SwaggerConfig, $"{AppData.AppName} v.{AppVersion}");

            // ATTENTION!
            // If you use are git repository then you can uncomment line with "ThisAssembly" below for versioning by GIT possibilities.
            // settings.HeadContent = $"{ThisAssembly.Git.Branch.ToUpper()} {ThisAssembly.Git.Commit.ToUpper()}";

            settings.DocumentTitle = $"{AppData.AppName}";
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelRendering(ModelRendering.Model);
            settings.DefaultModelsExpandDepth(0);
            settings.DocExpansion(DocExpansion.None);
            settings.DisplayRequestDuration();
        });
    }

    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppData.AppName,
                Version = AppVersion,
                Description = AppData.AppDescription
            });

            options.ResolveConflictingActions(x => x.First());
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Cookie",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Cookie",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}