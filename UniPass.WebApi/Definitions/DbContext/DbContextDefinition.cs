using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;

namespace UniPass.WebApi.Definitions.DbContext;

/// <summary>
///     ASP.NET Core services registration and configurations
/// </summary>
public class DbContextDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current application
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(config =>
        {
            // UseInMemoryDatabase - This for demo purposes only!
            // Should uninstall package "Microsoft.EntityFrameworkCore.InMemory" and install what you need.
            // For example: "Microsoft.EntityFrameworkCore.SqlServer"
            config.UseInMemoryDatabase("DEMO");

            // uncomment line below to use UseNpgsql() or UseSqlServer(). Don't forget setup connection string in appSettings.json
            // config.UseMssql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
            
            // config.UseSqlServer(builder.Configuration.GetConnectionString("UniPass"));
        });


        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
            })
            .AddSignInManager() //TODO: 
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        builder.Services.AddTransient<ApplicationUserStore>();
    }
}