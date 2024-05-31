using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Services;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public abstract class CrudController<TEntity, TKey> : Controller, ICrud<TEntity, TKey> where TEntity : Entity<TKey>
{
    protected readonly Repository<TEntity, TKey> Repository;
    protected readonly  UniPassBaseLogger<CrudController<TEntity, TKey>> Logger;

    protected CrudController(Repository<TEntity, TKey> repository, UniPassBaseLogger<CrudController<TEntity, TKey>> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    [HttpPost]
    public virtual async Task<Operation<TEntity>> Create([FromBody] TEntity entity)
    {
        try
        {
            if (entity == null) throw new UniPassApiException("Объект не может быть пуст");

            var createdEntity = await Repository.Create(entity);
            return Operation<TEntity>.Result(createdEntity,"Сохранено");
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<TEntity>.Error(e.Message);
        }
    }

    [HttpPost("/api/[controller]/many")]
    public virtual async Task<Operation<OperationInfo>> Create([FromBody] List<TEntity> entities)
    {
        try
        {
            var createdCount = await Repository.Create(entities);
            var info = new OperationInfo()
            {
                Count = createdCount,
            };
            return Operation<OperationInfo>.Result(info);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<OperationInfo>.Error(e.Message);
        }
    }

    [HttpDelete]
    public virtual async Task<Operation<OperationInfo>> Delete(TKey key)
    {
        try
        {
            var deletingEntity = Repository.GetFirstOrDefault(e => e.Id!.Equals(key));
            if (deletingEntity is null)
            {
                return Operation<OperationInfo>.Error($"Запись не была найдена с id={key}");
            }

            await Repository.Delete(deletingEntity);
            return Operation<OperationInfo>.Result("Запись успешно удалена");
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<OperationInfo>.Error(e.Message);
        }
    }

    [HttpGet("/api/[controller]/{page:int}/{pageSize:int}")]
    public virtual async Task<Operation<PagedList<TEntity>>> Read(int page, int pageSize)
    {
        try
        {
            var query = Repository
                .Read(e => true).Skip(page * pageSize).Take(pageSize);

            var count = await Repository.GetCount();
            var items = await query.ToListAsync();
            
            var pagedList = new PagedList<TEntity>(page, pageSize, count, items);

            return Operation<PagedList<TEntity>>.Result(pagedList);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<PagedList<TEntity>>.Error(e.Message);
        }
    }

    [HttpGet("{key}")]
    public virtual async Task<Operation<TEntity>> ReadFirst(TKey key, [FromQuery] string? includes)
    {
        try
        {
            var desiredEntity = includes is null ? Repository.GetFirstOrDefault(e => e.Id!.Equals(key)) :
                await Repository.GetIncludingRelatedEntities(includes).FirstOrDefaultAsync(e => e.Id!.Equals(key));
            
            if (desiredEntity is null)
            {
                return Operation<TEntity>.Error($"Запись не была найдена с id={key}");
            }
            return Operation<TEntity>.Result(desiredEntity);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<TEntity>.Error(e.Message);
        }
    }

    [HttpPut]
    public virtual async Task<Operation<TEntity>> Update([FromBody] TEntity entity)
    {
        try
        {
            if (entity == null || entity.Id is null) throw new UniPassApiException("Невалидный объект");

            var updatedEntity = await Repository.Update(entity);
            return Operation<TEntity>.Result(updatedEntity, "Запись успешно обновлена");
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<TEntity>.Error(e.Message);
        }
    }
}