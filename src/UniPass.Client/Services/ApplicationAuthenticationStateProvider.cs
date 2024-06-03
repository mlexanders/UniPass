using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using UniPass.Client.Services.Api;
using UniPass.Client.Utils;
using UniPass.Infrastructure;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services;

public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AccountService _accountService;
    private readonly UniPassClientLogger<ApplicationAuthenticationStateProvider> _logger;

    public ApplicationAuthenticationStateProvider(AccountService accountService,
        UniPassClientLogger<ApplicationAuthenticationStateProvider> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        try
        {
            var state = await GetAuthenticationState();

            if (state.IsAuthenticated)
                identity = new ClaimsIdentity(state?.Claims?
                    .Select(c => new Claim(c.Type, c.Value)), AppData.AppName);
        }
        catch (HttpRequestException ex)
        {
        }

        var result = new AuthenticationState(new ClaimsPrincipal(identity));
        return result;
    }

    public async Task Login(LoginViewModel model)
    {
        var result = await _accountService.Login(model);
        if (result.Value) NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        else throw new UniPassClientException(result.Message);
    }

    private async Task<ApplicationAuthenticationState> GetAuthenticationState()
    {
        try
        {
            return await _accountService.GetState();
        }
        catch (Exception e)
        {
            _logger.Log(e);
            return new ApplicationAuthenticationState();
        }
    }

    public async Task<UserModelViewModel> GetUser()
    {
        var result = await _accountService.GetCurrentUser();
        if (result.Success) return result.Value;
        throw new UniPassClientException(result.Message);
    }

    public async Task Logout()
    {
        await _accountService.Logout();
        NotifyAuthenticationStateChanged(GetAnonymousState());
    }

    private static async Task<AuthenticationState> GetAnonymousState()
    {
        return new AuthenticationState(new ClaimsPrincipal());
    }
}