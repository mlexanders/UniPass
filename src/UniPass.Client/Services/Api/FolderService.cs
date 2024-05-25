using UniPass.Client.Utils;
using UniPass.Domain;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class FolderService : BaseCrudRequests<Folder, int>, IFolderService
{
    public FolderService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<Operation<List<Folder>>> GetAllFolders()
    {
        var response = await Client.GetAsync($"{_basePath}/{nameof(Folder)}/all");
        return await response.GetResult<Operation<List<Folder>>>();
    }
}