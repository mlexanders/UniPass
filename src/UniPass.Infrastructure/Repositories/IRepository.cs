using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using UniPass.Domain.Base;

namespace UniPass.Infrastructure.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity> Create(TEntity entity);
    Task<int> Create(IEnumerable<TEntity> entities);

    IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes);

    TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool ignoreAutoIncludes = false);

    Task<TEntity> Update(TEntity entity);
    Task Update(params TEntity[] entities);
    Task Delete(TEntity entity);

    Task<int> GetCount(); 
    IQueryable<TEntity> GetAllIncludingRelatedEntities();
    IQueryable<TEntity> GetIncludingRelatedEntities(params string[] includes);
}