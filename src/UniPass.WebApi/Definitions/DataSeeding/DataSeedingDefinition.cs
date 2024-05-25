using Calabonga.AspNetCore.AppDefinitions;
using UniPass.WebApi.Definitions.DataSeeding.DatabaseInitialization;

namespace UniPass.WebApi.Definitions.DataSeeding;

public class DataSeedingDefinition : AppDefinition
{
    public override async void ConfigureApplication(WebApplication app)
    {
        await DatabaseInitializer.SeedUsers(app.Services);
        
        await DatabaseInitializer.SeedTeams(app.Services);
        DatabaseInitializer.SeedKeys(app.Services);
    }
}