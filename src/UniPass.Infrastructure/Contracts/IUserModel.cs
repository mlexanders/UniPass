namespace UniPass.Infrastructure.Contracts;

public interface IUserModel : IEntity<Guid>
{
    string? Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
}