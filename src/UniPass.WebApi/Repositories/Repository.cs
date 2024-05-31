using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Contracts;

namespace UniPass.WebApi.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        var result = DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<int> Create(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
        return await DbContext.SaveChangesAsync();
    }

    public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        IQueryable<TEntity> query = DbSet;
        if (includes != null)
            query = Include(query, includes);

        return query.Where(predicate);
    }

    public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool ignoreAutoIncludes = false)
    {
        IQueryable<TEntity> query = DbSet;

        if (disableTracking) query = query.AsNoTracking();

        if (include is not null) query = query.Include(include);

        if (predicate is not null) query = query.Where(predicate);

        if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

        if (ignoreAutoIncludes) query = query.IgnoreAutoIncludes();

        return orderBy is not null
            ? orderBy(query).FirstOrDefault()
            : query.FirstOrDefault();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        var result = DbSet.Update(entity);

        await DbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Update(params TEntity[] entities)
    {
        DbSet.UpdateRange(entities);
        await DbContext.SaveChangesAsync();
    }

    public async Task Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public Task<int> GetCount() => DbSet.CountAsync();

    protected IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[]? includes)
    {
        if (includes == null) return query;
        return includes.Aggregate(query, (current, include) => current.Include(include));
    }
    
    public IQueryable<TEntity> GetAllIncludingRelatedEntities()
    {
        IQueryable<TEntity> query = DbSet;
        foreach (var property in DbSet.EntityType.GetNavigations())
        {
            query = query.Include(property.Name);
        }

        return query;
    }
    
    public IQueryable<TEntity> GetIncludingRelatedEntities(params string[] includes)
    {
        IQueryable<TEntity> query = DbSet;
        foreach (var propertyName in includes)
        {
            query = query.Include(propertyName);
        }

        return query;
    }
    
    public IQueryable<TEntity> GetIncludingRelatedEntities(IQueryable<TEntity> query, params string[] includes)
    {
        foreach (var propertyName in includes)
        {
            query = query.Include(propertyName);
        }

        return query;
    }
    
    protected void MarkModified(TEntity entity)
    {
        var collections = DbContext.Entry(entity).Collections;
        foreach (var collection in collections)
        {
            if (collection.CurrentValue == null) continue;
            foreach (var element in collection.CurrentValue)
            {
                if (DbContext.Entry(element).State == EntityState.Unchanged)
                {
                    DbContext.Entry(element).State = EntityState.Modified;
                }
            }
        }

        foreach (var element in DbContext.Entry(entity).References)
        {
            if (element.CurrentValue == null)
            {
                continue;
            }

            if (DbContext.Entry(element.CurrentValue).State == EntityState.Unchanged)
            {
                DbContext.Entry(element.CurrentValue).State = EntityState.Modified;
            }
        }
    }

    // protected IQueryable<TEntity> IncludProperties(Expression<Func<TEntity, object>>[] includeProperties)
    // {
    //     return includeProperties.Aggregate(DbSet.AsNoTracking(), (query, includeProperty) => query.Include(includeProperty));
    // }
}