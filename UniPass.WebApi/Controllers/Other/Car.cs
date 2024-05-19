using UniPass.Domain.Base;

namespace UniPass.WebApi.Controllers;

public class Car : Entity<string>
{
    // public string Id { get; set; } = null!;
    public Motor Motor { get; set; }
}