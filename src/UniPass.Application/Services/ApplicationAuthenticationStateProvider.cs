using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using UniPass.Application.Services.Api;
using UniPass.Application.Utils;
using UniPass.Domain.Application;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Application.Services;

public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AccountService _accountService;
    private readonly UniPassClientLogger<ApplicationAuthenticationStateProvider> _logger;

    private bool _isAuthenticated = false;

    public ApplicationAuthenticationStateProvider(AccountService accountService, UniPassClientLogger<ApplicationAuthenticationStateProvider> logger)
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
        _isAuthenticated = result.User.Identity?.IsAuthenticated ?? false;
        Console.WriteLine($"RESULT = {result.User.Identity?.IsAuthenticated}");
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

    public async Task<UserViewModel> GetUser()
    {
        var result = await _accountService.GetCurrentUser();
        if (result.Success) return result.Value;
        throw new UniPassClientException(result.Message);
    }

    public async Task Logout()
    {
        await _accountService.Logout();
        var result = new AuthenticationState(new ClaimsPrincipal());

        NotifyAuthenticationStateChanged(GetAnonymousState());
    }

    private static async Task<AuthenticationState> GetAnonymousState() => new(new ClaimsPrincipal());

    public bool IsAuthenticated() => _isAuthenticated;
}