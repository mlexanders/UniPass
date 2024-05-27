using UniPass.Domain;
using UniPass.Infrastructure.Models;

namespace UniPass.Client.Utils;

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
    
    public static Folder Copy(this Folder folder)
    {
        var result = new Folder
        {
            Id = folder.Id,
            Name = folder.Name,
            Keys = folder.Keys,
            OwnerId = folder.OwnerId,
        };
        return result;
    }
    
    public static void Map(this Folder folder, Folder source)
    {
        folder.Id = source.Id;
        folder.Name = source.Name;
        folder.OwnerId = source.OwnerId;
        folder.Keys = source.Keys;
    }
    
    public static Team Copy(this Team team)
    {
        var result = new Team
        {
            Id = team.Id,
            Name = team.Name,
            OrganizerId = team.OrganizerId,
            Organizer = team.Organizer,
            Workers = team.Workers,

        };
        return result;
    }
    
    public static void Map(this Team team, Team source)
    {
        team.Id = source.Id;
        team.Name = source.Name;
        team.OrganizerId = source.OrganizerId;
        team.Organizer = source.Organizer;
        team.Workers = source.Workers;
    }
}