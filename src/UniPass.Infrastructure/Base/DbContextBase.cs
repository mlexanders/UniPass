﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Models;

namespace UniPass.Infrastructure.Base;

public abstract class DbContextBase : IdentityDbContext<ApplicationUserModel, ApplicationRole, Guid>
{
    protected DbContextBase(DbContextOptions options) : base(options)
    {
    }
}