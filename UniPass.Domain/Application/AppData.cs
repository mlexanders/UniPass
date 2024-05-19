namespace UniPass.Domain.Application;

public class AppData
{
    public static readonly IEnumerable<string> RoleNames = ["ApplicationUser"];
    public static string PolicyCorsName = "UniPass.CorcPolicy";
    public static string AppName = "UniPass";
    public static string AppDescription { get; set; }
}