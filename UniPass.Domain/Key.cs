using UniPass.Domain.Base;

namespace UniPass.Domain;

public class Key : Entity<Guid>, IKey
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}