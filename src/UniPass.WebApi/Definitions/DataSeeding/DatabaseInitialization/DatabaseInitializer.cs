﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniPass.Domain.Application;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;
using UniPass.WebApi.Repositories;

namespace UniPass.WebApi.Definitions.DataSeeding.DatabaseInitialization;

/// <summary>
///     Database Initializer
/// </summary>
public static class DatabaseInitializer
{
    private static readonly List<string> Emails = ["test@mymail.com", "a@ya.ru", "book@ya.ru", "yoyo@aya.ru"];

    // private static string _userName = "test@mymail.com";
    // private static string _userNameUpper = "TEST@MYMAIL.COM";
    /// <summary>
    ///     Seeds one default users to database for demo purposes only
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static async void SeedUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // ATTENTION!
        // -----------------------------------------------------------------------------
        // This is should not be used when UseInMemoryDatabase()
        // It should be uncomment when using UseSqlServer() settings or any other providers.
        // -----------------------------------------------------------------------------
        // await context!.Database.EnsureCreatedAsync();
        // var pending = await context.Database.GetPendingMigrationsAsync();
        // if (pending.Any())
        // {
        //     await context!.Database.MigrateAsync();
        // }

        if (context.Users.Any()) return;

        var roles = AppData.RoleNames.ToArray();

        foreach (var role in roles)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            if (!context!.Roles.Any(r => r.Name == role))
                await roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper() });
        }

        #region developer

        
        foreach (var email in Emails)
        {
            var user = new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                FirstName = email.Split("@")[0],
                LastName = "testov",
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "8977 777 77 77",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ApplicationUserProfile = new ApplicationUserProfile
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = "SEED",
                    Permissions = new List<AppPermission>
                    {
                        new()
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "SEED",
                            PolicyName = "Profiles:Roles:Get",
                            Description = "Access policy for view Roles in user Profiles"
                        }
                    }
                }
            };
            
            await CreateUser(context, user, scope, roles);
        }

        #endregion

        await context.SaveChangesAsync();
    }

    private static async Task CreateUser(ApplicationDbContext context, ApplicationUser user, IServiceScope scope,
        string[] roles)
    {
        if (!context!.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, "123qwe!@#");
            user.PasswordHash = hashed;
            var userStore = scope.ServiceProvider.GetRequiredService<ApplicationUserStore>();
            var result = await userStore.CreateAsync(user);
            if (!result.Succeeded) throw new InvalidOperationException("Cannot create account");

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            foreach (var role in roles)
            {
                var roleAdded = await userManager!.AddToRoleAsync(user, role);
                if (roleAdded.Succeeded) await context.SaveChangesAsync();
            }
        }
    }

    public static async void SeedTeams(IServiceProvider appServices)
    {
        var teamsRepository = appServices.CreateScope()
            .ServiceProvider.GetRequiredService<TeamsRepository>();
        
        var context = appServices.CreateScope()
            .ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = context.Users.FirstOrDefault(u => u.UserName!.Equals(Emails[0]));
        var users = context.Users.Where(u => Emails.Contains(u.Email!)).ToList();

        
        var teams = new List<Team>()
        {
            new Team()
            {
                Name = "Регард",
                OrganizerId = user!.Id,
                Workers = users
            },
            new Team()
            {
                Name = "ДНС",
                OrganizerId = user!.Id,
                // Workers = users
            },

            new Team()
            {
                Name = "Оле",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Хавей",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Пятерочка",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Ситилинк",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Райдер",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Перекресток",
                OrganizerId = user!.Id,
            },
            new Team()
            {
                Name = "Самсунг",
                OrganizerId = user!.Id,
            },
        };
        
         context.Teams.AddRange(teams);
         await context.SaveChangesAsync();
    }

    // public static async void SeedEvents(IServiceProvider serviceProvider)
    // {
    //     using var scope = serviceProvider.CreateScope();
    //     await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //
    //     // ATTENTION!
    //     // -----------------------------------------------------------------------------
    //     // This is should not be used when UseInMemoryDatabase()
    //     // It should be uncomment when using UseSqlServer() settings or any other providers.
    //     // -----------------------------------------------------------------------------
    //     //await context!.Database.EnsureCreatedAsync();
    //     //var pending = await context.Database.GetPendingMigrationsAsync();
    //     //if (pending.Any())
    //     //{
    //     //    await context!.Database.MigrateAsync();
    //     //}
    //
    //     if (context..Any())
    //     {
    //         return;
    //     }
    //
    //     await context.EventItems.AddAsync(new EventItem
    //     {
    //         CreatedAt = DateTime.UtcNow,
    //         Id = Guid.Parse("1467a5b9-e61f-82b0-425b-7ec75f5c5029"),
    //         Level = "Information",
    //         Logger = "SEED",
    //         Message = "Seed method some entities successfully save to ApplicationDbContext"
    //     });
    //
    //     await context.SaveChangesAsync();
    // }

}