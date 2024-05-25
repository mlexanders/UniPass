using UniPass.Domain.Base;

namespace UniPass.Domain;

public class Folder : Entity<int>
{
    public string Name { get; set; }
    
    public List<Key> Keys { get; set; }
    public Guid OwnerId  { get; set; }
}