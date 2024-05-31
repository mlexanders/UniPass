using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class AccountController : Controller, IAccount
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUserModel> _signInManager;

    public AccountController(
        SignInManager<ApplicationUserModel> signInManager,
        ILogger<AccountController> logger,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    public async Task<Operation<bool>> Login([FromBody] LoginViewModel model)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
        if (user is null) return Operation<bool>.Error("Учетная запись не найдена");

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded) return Operation<bool>.Result(true, "Вы вошли в систему");

        return Operation<bool>.Error("Неверный логин или пароль");
    }

    [HttpGet]
    [Route("/api/[controller]/State")]
    public Task<ApplicationAuthenticationState> GetState()
    {
        return Task.FromResult(new ApplicationAuthenticationState
        {
            IsAuthenticated = User?.Identity?.IsAuthenticated ?? false,
            Name = User?.Identity?.Name,
            Claims = User?.Claims.Select(c => new ApplicationClaim { Type = c.Type, Value = c.Value })
        });
    }

    [HttpGet]
    [Route("/api/[controller]/CurrentUser")]
    public async Task<Operation<UserModelViewModel>> GetCurrentUser()
    {
        try
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new UniPassApiException("Не авторизован");

            var user = await _signInManager.UserManager.FindByIdAsync(userId)
                       ?? throw new UniPassApiException("Учетная запись не найдена");

            var userModel = _mapper.Map<UserModelViewModel>(user);
            return Operation<UserModelViewModel>.Result(userModel);
        }
        catch (UniPassApiException e)
        {
            return Operation<UserModelViewModel>.Error(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message, User);
            return Operation<UserModelViewModel>.Error(e.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<Operation<bool>> Logout()
    {
        var res = HttpContext.User.Identity?.IsAuthenticated;
        await _signInManager.SignOutAsync();
        return Operation<bool>.Result(true);
    }
}