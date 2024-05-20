namespace UniPass.WebApi.Utils;

public class UniPassApiException : Exception
{
    public UniPassApiException(string msg) : base(msg)
    {
    }
}