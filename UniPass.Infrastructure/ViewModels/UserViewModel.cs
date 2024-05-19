using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.ViewModels;

public class UserViewModel : IUser
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}