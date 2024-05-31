namespace UniPass.Infrastructure.Contracts;

public class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
}