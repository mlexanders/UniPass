using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;

namespace UniPass.WebApi.Repositories;

public class KeysRepository : Repository<Key, Guid>
{
    public KeysRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}