using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Models;

namespace UniPass.Infrastructure;

/// <summary>
///     Application store for user
/// </summary>
public class ApplicationUserStore : UserStore<ApplicationUserModel, ApplicationRole, ApplicationDbContext, Guid>
{
    public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer)
        : base(context, describer)
    {
    }

    // public override Task<ApplicationUserModel?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    // {
    //     return Users
    //         .Include(x => x.ApplicationUserProfile).ThenInclude(x => x!.Permissions)
    //         .FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken)!;
    // }

    // public override Task<ApplicationUserModel?> FindByNameAsync(string normalizedUserName,
    //     CancellationToken cancellationToken = default)
    // {
    //     return Users
    //         .Include(x => x.ApplicationUserProfile).ThenInclude(x => x!.Permissions)
    //         .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName,
    //             cancellationToken)!;
    // }
}