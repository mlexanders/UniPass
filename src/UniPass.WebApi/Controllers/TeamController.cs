using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.Services;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Authorize]
public class TeamController : CrudController<Team, Guid>
{
    private readonly TeamsRepository _repository;

    public TeamController(TeamsRepository repository, UniPassBaseLogger<CrudController<Team, Guid>> logger)
        : base(repository, logger)
    {
        _repository = repository;
    }

    public override async Task<Operation<PagedList<Team>>> Read(int page, int pageSize)
    {
        try
        {
            var userId = User.GetUserId();
            
            var query = Repository
                .Read(e => e.OrganizerId.Equals(userId))
                .Skip(page * pageSize)
                .Take(pageSize);

            var count = await Repository.GetCount();
            var items = await query.ToListAsync();
            
            var pagedList = new PagedList<Team>(page, pageSize, count, items);

            return Operation<PagedList<Team>>.Result(pagedList);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<PagedList<Team>>.Error(e.Message);
        }
    }

    public override async Task<Operation<Team>> ReadFirst(Guid key, string? includes)
    {
        try
        {
            var userId = User.GetUserId();

            var desiredEntity = includes is null ? Repository.GetFirstOrDefault(e => e.Id!.Equals(key) && e.OrganizerId.Equals(userId)) :
                await Repository.GetIncludingRelatedEntities(includes).FirstOrDefaultAsync(e => e.Id!.Equals(key) && e.OrganizerId.Equals(userId));
            
            if (desiredEntity is null)
            {
                return Operation<Team>.Error($"Запись не была найдена с id={key}");
            }
            return Operation<Team>.Result(desiredEntity);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Team>.Error(e.Message);
        }
    }

    [HttpPost("deleteWorker")]
    public async Task<Operation<bool>> DeleteWorker([FromQuery] Guid teamId, [FromQuery] Guid workerId)
    {
        try
        {
            var success = await _repository.DeleteWorker(teamId, workerId);
            return Operation<bool>.Result(success);
        }
        catch (UniPassApiException e)
        {
            return Operation<bool>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return Operation<bool>.Error(e.Message);
        }
    }

    public override async Task<Operation<Team>> Update(Team entity)
    {
        try
        {
            var userId = User.GetUserId();

            var updated = _repository.GetFirstOrDefault(e => e.Id.Equals(e.Id) && e.OrganizerId.Equals(userId));
            if (updated is null)
            {
                throw new UniPassApiException("Команда не найдена");
            }

            var result = await _repository.Update(entity);
            return Operation<Team>.Result(result);
        }
        catch (UniPassApiException e)
        {
            return Operation<Team>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return Operation<Team>.Error(e.Message);
        }
    }
}