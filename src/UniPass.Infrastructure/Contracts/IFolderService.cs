using UniPass.Domain;
using UniPass.Infrastructure.Repositories;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Infrastructure.Contracts;

public interface IFolderService : ICrud<Folder, int>
{
    Task<Operation<List<Folder>>> GetAllFolders();
    Task<Operation<List<Folder>>> GetByTeamId(Guid teamId);
}