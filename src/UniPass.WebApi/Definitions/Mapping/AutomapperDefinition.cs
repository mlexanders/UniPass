using Calabonga.AspNetCore.AppDefinitions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace UniPass.WebApi.Definitions.Mapping;

public class AutomapperDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));
    }

    public override void ConfigureApplication(WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<IConfigurationProvider>();
        if (app.Environment.IsDevelopment()) //TODO: 
            mapper.AssertConfigurationIsValid();
        else
            mapper.CompileMappings();
    }
}