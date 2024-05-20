namespace UniPass.Domain.Base;

public interface IKey
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}