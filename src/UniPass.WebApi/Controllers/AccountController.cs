using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class AccountController : Controller, IAccountService
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
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
        if (result.Succeeded) return Operation<bool>.Result(true);

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
    public async Task<Operation<UserViewModel>> GetCurrentUser()
    {
        try
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new UniPassApiException("Не авторизован");

            var user = await _signInManager.UserManager.FindByIdAsync(userId)
                       ?? throw new UniPassApiException("Учетная запись не найдена");

            var userModel = _mapper.Map<UserViewModel>(user);
            return Operation<UserViewModel>.Result(userModel);
        }
        catch (UniPassApiException e)
        {
            return Operation<UserViewModel>.Error(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message, User);
            return Operation<UserViewModel>.Error(e.Message);
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