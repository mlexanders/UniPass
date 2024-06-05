using Microsoft.AspNetCore.Identity;
using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.Models;

public class ApplicationUserModel : IdentityUser<Guid>, IUserModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual ICollection<Team>? Teams { get; set; }
    public virtual ICollection<Team>? CreatedTeams { get; set; }   
    public virtual ICollection<Folder>? Folders { get; set; }    
}