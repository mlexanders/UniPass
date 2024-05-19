using UniPass.Domain.Base;

namespace UniPass.Infrastructure.Contracts;

public interface IUser : IEntity<Guid>
{
    string? Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
}