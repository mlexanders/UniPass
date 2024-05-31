using UniPass.Infrastructure.Contracts;

namespace UniPass.Infrastructure.Models;

public class Folder : Entity<int>
{
    public string Name { get; set; }
    
    public List<Key> Keys { get; set; }
    public Guid OwnerId  { get; set; }
    public Guid? TeamId  { get; set; }
}