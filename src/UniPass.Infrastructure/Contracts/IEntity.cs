namespace UniPass.Infrastructure.Contracts;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}