﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class UserController : Controller, IUser
{
    private readonly UserManager<ApplicationUserModel> _userManager;

    public UserController(UserManager<ApplicationUserModel> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<Operation<bool>> Register([FromBody] RegisterViewModel model)
    {
        try
        {
            var newUser = new ApplicationUserModel
            {
                UserName = model.Email,
                NormalizedUserName = null,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                return Operation<bool>.Result(true, "ВЫ успешно зарегистрированы");
            }

            var firstError = result.Errors.FirstOrDefault()?.Description ?? "Ошибка";
            return Operation<bool>.Result(false, firstError);
        }
        catch (Exception e)
        {
            return Operation<bool>.Result(false, e.Message);
        }
    }
}