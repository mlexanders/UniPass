using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Contracts;

public interface ICrud<TEntity, in TKey> where TEntity : Entity<TKey>
{
    Task<Operation<TEntity>> Create(TEntity entity);
    Task<Operation<OperationInfo>> Create(List<TEntity> entities);
    Task<Operation<TEntity>> ReadFirst(TKey key, string? includes);
    Task<Operation<PagedList<TEntity>>> Read(int page, int pageSize);
    Task<Operation<TEntity>> Update(TEntity entity);
    Task<Operation<OperationInfo>> Delete(TKey key);
}