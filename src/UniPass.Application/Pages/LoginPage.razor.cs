using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using UniPass.Application.Services;
using UniPass.Application.Utils;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Application.Pages;

public partial class LoginPage
{
    private readonly LoginViewModel _model = new();
    private string _error;
    private bool _errorVisible;
    private string _info;
    private bool _infoVisible;

    [Inject] private ApplicationAuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ILogger<LoginPage> _Logger { get; set; }

    private async Task Submit(LoginArgs args)
    {
        var model = new LoginViewModel
        {
            Email = args.Username,
            Password = args.Password
        };

        try
        {
            await AuthenticationStateProvider.Login(model);
            NavigationManager.NavigateTo("/", true);
        }
        catch (HttpRequestException e)
        {
            _errorVisible = true;
            _error = "Ошибка сети";
            _Logger.LogError(e.Message);   
            _Logger.LogError(e.StackTrace);   
        }
        catch (Exception e)
        {
            _Logger.LogError(e.Message);   
            _Logger.LogError(e.StackTrace);   
            _errorVisible = true;
            _error = e.Message;
        }
    }

    private async Task OnRegister()
    {
    }

    private async Task OnReset()
    {
    }
}