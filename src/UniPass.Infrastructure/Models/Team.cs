using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.Models;

public class Team : Entity<Guid>
{
    public string Name { get; set; }
    public Guid OrganizerId { get; set; }
    public virtual ApplicationUserModel Organizer { get; set; }
    
    public virtual ICollection<ApplicationUserModel>? Workers { get; set; }
    public virtual ICollection<Folder>? Folders { get; set; }
}