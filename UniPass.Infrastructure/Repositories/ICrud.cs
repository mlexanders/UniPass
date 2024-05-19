using UniPass.Domain.Base;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Repositories;

public interface ICrud<TEntity, in TKey> where TEntity : Entity<TKey>
{
    Task<Operation<TEntity>> Create(TEntity entity);
    Task<Operation<OperationInfo>> Create(List<TEntity> entities);
    Task<Operation<TEntity>> ReadFirst(TKey key, string? includes);
    Task<Operation<PagedList<TEntity>>> Read(int page, int pageSize);
    Task<Operation<TEntity>> Update(TEntity entity);
    Task<Operation<OperationInfo>> Delete(TKey key);
}

public class OperationInfo
{
    public int Count { get; set; }
    public List<string>? Messages { get; set;  }
}