using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniPass.Domain;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Repositories;

namespace UniPass.WebApi.Repositories;

public class FoldersRepository : Repository<Folder, int>
{
    public FoldersRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    
    public IQueryable<Folder> ReadByOwnerId(Guid ownerId, Expression<Func<Folder, bool>> predicate)
    {
        return DbSet
            .Include(f => f.Keys)
            .Where(f => f.OwnerId.Equals(ownerId))
            .Where(predicate);
    }
}