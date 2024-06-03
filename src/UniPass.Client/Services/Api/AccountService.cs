using System.Net.Http.Json;
using UniPass.Client.Utils;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class AccountService : IAccount
{
    private readonly string _basePath;
    private readonly HttpClient _client;

    public AccountService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(AppData.AppName);
        _basePath = _client.BaseAddress?.ToString();
    }

    public async Task<Operation<bool>> Login(LoginViewModel model)
    {
        var response = await _client.PostAsJsonAsync($"{_basePath}/Account/Login", model);
        return await response.GetResult<Operation<bool>>();
    }

    public async Task<ApplicationAuthenticationState> GetState()
    {
        var response = await _client.GetAsync($"{_basePath}/Account/State");
        return await response.GetResult<ApplicationAuthenticationState>();
    }

    public async Task<Operation<UserModelViewModel>> GetCurrentUser()
    {
        var response = await _client.GetAsync($"{_basePath}/Account/CurrentUser");
        return await response.GetResult<Operation<UserModelViewModel>>();
    }

    public async Task<Operation<bool>> Logout()
    {
        var response = await _client.GetAsync($"{_basePath}/Account/Logout");
        return await response.GetResult<Operation<bool>>();
    }
}