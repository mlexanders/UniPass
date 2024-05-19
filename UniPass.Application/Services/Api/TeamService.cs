using System.Net.Http.Json;
using UniPass.Application.Utils;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Models;

namespace UniPass.Application.Services.Api;

public class TeamService : BaseCrudRequests<Team, Guid>
{
    public TeamService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public async Task<Operation<bool>> DeleteWorker(Guid teamId, Guid workerId)
    {
        var uri = $"{_basePath}/{nameof(Team)}/deleteWorker?teamId={teamId}&workerId={workerId}";
        var response = await Client.PostAsJsonAsync(uri, "");
        return await response.GetResult<Operation<bool>>();
    }
}