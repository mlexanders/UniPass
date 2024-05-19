using Microsoft.AspNetCore.Identity;
using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.Models;

public class ApplicationUser : IdentityUser<Guid>, IUser
{
    public Guid? ApplicationUserProfileId { get; set; }
    public virtual ApplicationUserProfile? ApplicationUserProfile { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual ICollection<Team>? Teams { get; set; }
    public virtual ICollection<Team>? CreatedTeams { get; set; }    
    // public virtual ICollection<ApplicationUserTeam>? ApplicationUserTeam { get; set; }

}