using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPass.Domain;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Repositories;
using UniPass.Infrastructure.Services;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Authorize]
public class FolderController : CrudController<Folder, int>, IFolderService
{
    private readonly FoldersRepository _repository;

    public FolderController(FoldersRepository repository, UniPassBaseLogger<CrudController<Folder, int>> logger) : base(
        repository, logger)
    {
        _repository = repository;
    }

    public override async Task<Operation<Folder>> Create(Folder entity)
    {
        try
        {
            if (entity == null) throw new UniPassApiException("Невалидный объект");
            entity.OwnerId =  User.GetUserId();;
            
            return await base.Create(entity);
        }
        catch (UniPassApiException e)
        {
            return Operation<Folder>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Folder>.Error(e.Message);
        }
    }

    public override async Task<Operation<OperationInfo>> Create(List<Folder> entities)
    {
        try
        {
            if (entities == null) throw new UniPassApiException("Невалидный объект");

            var creatorUserId = User.GetUserId();
            
            foreach (var entity in entities)
            {
                entity.OwnerId = creatorUserId;
            }

            return await base.Create(entities);
        }
        catch (UniPassApiException e)
        {
            return Operation<OperationInfo>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<OperationInfo>.Error(e.Message);
        }
    }

    public override async Task<Operation<PagedList<Folder>>> Read(int page, int pageSize)
    {
        try
        {
            var userId = User.GetUserId();

            var allFolders =  await _repository
                .ReadByOwnerId(userId, f => true)
                .ToListAsync();
            
            var count = allFolders.Count;
            var folders = allFolders
                .Skip(page * pageSize).Take(pageSize).ToList();

            var pagedList = new PagedList<Folder>(page, pageSize, count, folders);

            return Operation<PagedList<Folder>>.Result(pagedList);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<PagedList<Folder>>.Error(e.Message);
        }
    }

    public override async Task<Operation<Folder>> ReadFirst(int key, string? includes)
    {
        try
        {
            var userId = User.GetUserId();

            var folder = await _repository.ReadByOwnerId(userId, f => true)
                .FirstOrDefaultAsync(f => f.Id.Equals(key));

            if (folder is null)
            {
                return Operation<Folder>.Error($"Запись не была найдена");
            }

            return Operation<Folder>.Result(folder);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Folder>.Error(e.Message);
        }
    }

    [HttpGet("all")]
    public async Task<Operation<List<Folder>>> GetAllFolders()
    {
        try
        {
            var userId = User.GetUserId();

            var folders = await _repository
                .ReadByOwnerId(userId, f =>true)
                .ToListAsync();
            

            return Operation<List<Folder>>.Result(folders);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<List<Folder>>.Error(e.Message);
        }
    }

    [HttpGet("GetByTeamId/{teamId:guid}")]
    public async Task<Operation<List<Folder>>> GetByTeamId(Guid teamId)
    {
        
        try
        {
            var folders = await _repository
                .ReadByOwnerId(User.GetUserId(), f => teamId.Equals(f.TeamId))
                .Include(f => f.Keys)
                .ToListAsync();
            
            return Operation<List<Folder>>.Result(folders);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return Operation<List<Folder>>.Error(e.Message);
        }
    }
}