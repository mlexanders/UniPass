using UniPass.Domain;
using UniPass.Domain.Base;

namespace UniPass.Infrastructure.Models;

public class Team : Entity<Guid>
{
    public string Name { get; set; }
    public Guid OrganizerId { get; set; }
    public virtual ApplicationUser Organizer { get; set; }
    
    public virtual ICollection<ApplicationUser>? Workers { get; set; }
    public virtual ICollection<Folder>? Folders { get; set; }
}