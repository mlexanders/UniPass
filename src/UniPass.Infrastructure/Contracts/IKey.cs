namespace UniPass.Infrastructure.Contracts;

public interface IKey
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
    public string Note { get; set; }
}