using Calabonga.AspNetCore.AppDefinitions;
using UniPass.WebApi.Definitions.DataSeeding.DatabaseInitialization;

namespace UniPass.WebApi.Definitions.DataSeeding;

public class DataSeedingDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        DatabaseInitializer.SeedUsers(app.Services);
        
        DatabaseInitializer.SeedTeams(app.Services);
    }
}