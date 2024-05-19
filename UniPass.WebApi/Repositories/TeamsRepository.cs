using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Repositories;

public class TeamsRepository : Repository<Team, Guid>
{
    public TeamsRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<bool> DeleteWorker(Guid teamId, Guid workerId)
    {
        var findedTeam = await DbSet.Include(i => i.Workers)
            .FirstOrDefaultAsync(t => t.Id.Equals(teamId));
        if (findedTeam is null )
        {
            throw new UniPassApiException($"Команда не найдена");
        }
        else if (findedTeam.Workers is null)
        {
            throw new UniPassApiException($"Список участников пуст");
        }
        
        var user = findedTeam.Workers.FirstOrDefault(u => u.Id.Equals(workerId));
        if (user is null )
        {
            throw new UniPassApiException($"Пользователь не найден");
        }
        
        var result = findedTeam.Workers.Remove(user);
        await DbContext.SaveChangesAsync();
        return result;
    }
}