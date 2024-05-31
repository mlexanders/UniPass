namespace UniPass.Infrastructure;

public class AppData
{
    public static readonly string AppName = "UniPass";
    public static readonly string AppTittle = $"{AppName} v1.0.0, Copyright \u24b8 2024";
    public static readonly string AppDescription = "Web-API for UniPass Application.";
    
    public static readonly IEnumerable<string> RoleNames = ["ApplicationUser"];
    public static readonly string PolicyCorsName = "UniPass.CorsPolicy";
}