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

    public async Task<Team> DeleteWorker(Guid teamId, Guid workerId)
    {
        var updatedTeam = await DbSet
            .Include(i => i.Workers)
            .FirstOrDefaultAsync(t => t.Id.Equals(teamId));
        
        if (updatedTeam is null)
        {
            throw new UniPassApiException($"Команда не найдена");
        }
        else if (updatedTeam.Workers is null)
        {
            throw new UniPassApiException($"Список участников пуст");
        }

        var user = updatedTeam.Workers.FirstOrDefault(u => u.Id.Equals(workerId));
        if (user is null)
        {
            throw new UniPassApiException($"Пользователь не найден");
        }

        var result = updatedTeam.Workers.Remove(user);
        await DbContext.SaveChangesAsync();
        
        return result ? updatedTeam : throw new UniPassApiException("Пользователь не был удален");
    }
}