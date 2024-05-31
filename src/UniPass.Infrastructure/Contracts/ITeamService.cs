using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Contracts;

public interface ITeamService : ICrud<Team, Guid>
{
    Task<Operation<Team>> DeleteWorker(Guid teamId, Guid workerId);
    Task<Operation<Team>> AddWorker(Guid teamId, string email);
    Task<Operation<PagedList<Team>>> ReadAsParticipant(int page, int pageSize);
}