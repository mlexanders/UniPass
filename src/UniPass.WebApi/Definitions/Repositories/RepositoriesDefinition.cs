using Calabonga.AspNetCore.AppDefinitions;
using UniPass.WebApi.Repositories;

namespace UniPass.WebApi.Definitions.Repositories;

public class RepositoriesDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<TeamsRepository>();
        builder.Services.AddTransient<KeysRepository>();
        builder.Services.AddTransient<FoldersRepository>();
    }
}