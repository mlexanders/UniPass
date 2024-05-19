using UniPass.Domain.Base;

namespace UniPass.Infrastructure.Models;

public class AppPermission : Entity<Guid>
{
    public virtual ApplicationUserProfile? ApplicationUserProfile { get; set; }
    public string PolicyName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}