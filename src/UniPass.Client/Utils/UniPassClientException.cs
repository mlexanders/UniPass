using Microsoft.Win32.SafeHandles;
using Radzen;
using UniPass.Domain;

namespace UniPass.Client.Utils;

public class UniPassClientException : Exception
{
    public UniPassClientException(string resultMessage) : base(resultMessage)
    {
    }
    
    public UniPassClientException(NotificationMessage notificationMessage)
    {
        NotificationMessage = notificationMessage;
    }

    public NotificationMessage NotificationMessage { get; }
}


public static class Mapper
{
    public static Key Copy(this Key key)
    {
        var result = new Key
        {
            Id = key.Id,
            Name = key.Name,
            Login = key.Login,
            Password = key.Password,
            Url = key.Url,
            Note = key.Note,
            FolderId = key.FolderId,
            Folder = key.Folder
        };
        return result;
    }
    
    public static void Map(this Key key, Key source)
    {
        key.Id = source.Id;
        key.Name = source.Name;
        key.Login = source.Login;
        key.Password = source.Password;
        key.Url = source.Url;
        key.Note = source.Note;
        key.FolderId = source.FolderId;
        key.Folder = source.Folder;
    }
}