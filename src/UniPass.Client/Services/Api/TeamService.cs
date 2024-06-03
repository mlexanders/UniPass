using System.Net.Http.Json;
using UniPass.Client.Utils;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class TeamService : BaseCrudRequests<Team, Guid>, ITeamService
{
    public TeamService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<Operation<Team>> DeleteWorker(Guid teamId, Guid workerId)
    {
        var uri = $"{BasePath}/{nameof(Team)}/deleteWorker?teamId={teamId}&workerId={workerId}";
        var response = await Client.PostAsJsonAsync(uri, "");
        return await response.GetResult<Operation<Team>>();
    }

    public async Task<Operation<Team>> AddWorker(Guid teamId, string email)
    {
        var uri = $"{BasePath}/{nameof(Team)}/addWorker?teamId={teamId}&email={email}";
        var response = await Client.PostAsJsonAsync(uri, "");
        return await response.GetResult<Operation<Team>>();
    }

    public async Task<Operation<PagedList<Team>>> ReadAsParticipant(int page, int pageSize)
    {
        var uri = $"{BasePath}/{nameof(Team)}/ReadAsParticipant/{page}/{pageSize}";
        var response = await Client.GetAsync(uri);
        return await response.GetResult<Operation<PagedList<Team>>>();
    }

    public async Task<PagedList<Team>> GetTeamsPage(int pageSize, int? argsTop, int? argsSkip)
    {
        var skip = argsSkip ?? 0;
        var page = skip / pageSize;

        var result = await Read(page, argsTop ?? pageSize);

        if (result.Success) return result.Value;

        throw new UniPassClientException(result.Message);
    }

    public async Task<PagedList<Team>> GetTeamsPageAsParticipant(int pageSize, int? argsTop, int? argsSkip)
    {
        var skip = argsSkip ?? 0;
        var page = skip / pageSize;

        var result = await ReadAsParticipant(page, argsTop ?? pageSize);

        if (result.Success) return result.Value;

        throw new UniPassClientException(result.Message);
    }
}