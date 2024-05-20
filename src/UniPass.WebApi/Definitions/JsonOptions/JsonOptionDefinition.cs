using System.Text.Json.Serialization;
using Calabonga.AspNetCore.AppDefinitions;

namespace UniPass.WebApi.Definitions.JsonOptions;

public class JsonOptionDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }
}