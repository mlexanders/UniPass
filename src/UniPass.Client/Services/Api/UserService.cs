using System.Net.Http.Json;
using UniPass.Client.Utils;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class UserService : IUser
{
    private readonly HttpClient _client;
    private readonly string _entityPath;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(AppData.AppName);
        var basePath = _client.BaseAddress?.ToString();
        _entityPath = $"{basePath}/User";
    }

    public async Task<Operation<bool>> Register(RegisterViewModel model)
    {
        var response = await _client.PostAsJsonAsync($"{_entityPath}/Register", model);
        return await response.GetResult<Operation<bool>>();
    }
}