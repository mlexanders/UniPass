using System.Security.Claims;

namespace UniPass.WebApi.Utils;

public class UniPassApiException : Exception
{
    public UniPassApiException(string msg) : base(msg)
    {
    }
}

public static class ClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        var id = claim?.Value ?? throw new UniPassApiException("id пользователя не был найден");

        return Guid.Parse(id);
    }
}