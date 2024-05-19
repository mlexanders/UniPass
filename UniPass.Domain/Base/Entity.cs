namespace UniPass.Domain.Base;

public class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
}