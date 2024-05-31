using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;

namespace UniPass.WebApi.Repositories;

public class FoldersRepository : Repository<Folder, int>
{
    private readonly ApplicationDbContext _dbContext;

    public FoldersRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Folder> ReadByOwnerId(Guid ownerId, Expression<Func<Folder, bool>> predicate)
    {
        return DbSet
            .Include(f => f.Keys)
            .Where(f => f.OwnerId.Equals(ownerId))
            .Where(predicate);
    }

    public IQueryable<Folder> ReadAsParticipant(Guid participantId, Guid teamId)
    {
        return _dbContext.Teams
            .Where(t => t.Id.Equals(teamId) && t.Workers!.Any(u => u.Id.Equals(participantId)))
            .SelectMany(t => t.Folders!);
    }
}