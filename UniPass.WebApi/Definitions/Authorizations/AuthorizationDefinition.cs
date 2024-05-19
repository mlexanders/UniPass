using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UniPass.WebApi.Definitions.Authorizations;

public class AuthorizationDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme
            );


        builder.Services.AddAuthorization();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}