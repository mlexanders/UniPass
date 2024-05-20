using UniPass.Domain.Base;

namespace UniPass.Domain;

public class Folder : IEntity<int>
{
    public string Name { get; set; }
    public List<Folder> Folders { get; set; }
    public List<Key> Keys { get; set; }
    public int Id { get; set; }
}