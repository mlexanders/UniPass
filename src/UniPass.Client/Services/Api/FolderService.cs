using UniPass.Client.Utils;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class FolderService : BaseCrudRequests<Folder, int>, IFolderService
{
    public FolderService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<Operation<List<Folder>>> GetAllFolders()
    {
        var path = $"{BasePath}/{nameof(Folder)}/all";
        
        var response = await Client.GetAsync(path);
        return await response.GetResult<Operation<List<Folder>>>();
    }


    public async Task<Operation<List<Folder>>> GetByTeamId(Guid teamId, bool isOwner = true)
    {
        var path = $"{BasePath}/{nameof(Folder)}/GetByTeamId/{teamId}/{isOwner}";
        
        var response = await Client.GetAsync(path);
        return await response.GetResult<Operation<List<Folder>>>();
    }
}