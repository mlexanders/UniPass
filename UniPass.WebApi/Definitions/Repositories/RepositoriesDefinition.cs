using Calabonga.AspNetCore.AppDefinitions;
using UniPass.Infrastructure.Repositories;
using UniPass.WebApi.Repositories;

namespace UniPass.WebApi.Definitions.Repositories;

public class RepositoriesDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<TeamsRepository>();
    }
}