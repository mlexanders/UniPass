using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.Models;

public class ApplicationUserProfile : Entity<Guid>
{
    // public virtual ApplicationUser? ApplicationUser { get; set; }

    public List<AppPermission>? Permissions { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}