using Calabonga.AspNetCore.AppDefinitions;
using UniPass.Infrastructure.Services;

namespace UniPass.WebApi.Definitions.Logging;

public class LoggingDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(UniPassBaseLogger<>));
    }
}