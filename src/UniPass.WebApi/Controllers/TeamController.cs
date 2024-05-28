using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.Services;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Authorize]
public class TeamController : CrudController<Team, Guid>, ITeamService
{
    private readonly TeamsRepository _repository;
    private readonly ApplicationUserStore _userStore;

    public TeamController(TeamsRepository repository, ApplicationUserStore userStore,
        UniPassBaseLogger<CrudController<Team, Guid>> logger)
        : base(repository, logger)
    {
        _repository = repository;
        _userStore = userStore;
    }

    public override Task<Operation<Team>> Create(Team entity)
    {
        if (entity is not null)
        {
            entity.OrganizerId = User.GetUserId();
        }

        return base.Create(entity);
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

            var desiredEntity = includes is null
                ? Repository.GetFirstOrDefault(e => e.Id!.Equals(key) && e.OrganizerId.Equals(userId))
                : await Repository.GetIncludingRelatedEntities(includes)
                    .FirstOrDefaultAsync(e => e.Id!.Equals(key) && e.OrganizerId.Equals(userId));

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
    public async Task<Operation<Team>> DeleteWorker([FromQuery] Guid teamId, [FromQuery] Guid workerId)
    {
        try
        {
            var updatedTeam = await _repository.DeleteWorker(teamId, workerId);
            return Operation<Team>.Result(updatedTeam);
        }
        catch (UniPassApiException e)
        {
            return Operation<Team>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Team>.Error(e.Message);
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
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Team>.Error(e.Message);
        }
    }

    [HttpPost("addWorker")]
    public async Task<Operation<Team>> AddWorker([FromQuery] Guid teamId, [FromQuery] string email)
    {
        try
        {
            var userId = User.GetUserId();

            var user = await _userStore.FindByEmailAsync(email);
            var team = _repository
                .GetFirstOrDefault(t => t.Id.Equals(teamId) && t.OrganizerId.Equals(userId),
                    disableTracking: false,
                    include: t => t.Workers!);

            if (user is null | team is null)
            {
                throw new UniPassApiException("Пользвоатель или команда не была найдена");
            }

            if (team!.Workers?.Any() != true)
            {
                team!.Workers = [user];
            }
            else
            {
                team.Workers.Add(user!);
            }

            var result = await _repository.Update(team);

            //TODO: NOTIFICATION _ SERVICE

            return Operation<Team>.Result(result);
        }
        catch (UniPassApiException e)
        {
            return Operation<Team>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<Team>.Error(e.Message);
        }
    }

    [HttpGet("ReadAsParticipant/{page:int}/{pageSize:int}")]
    public async Task<Operation<PagedList<Team>>> ReadAsParticipant(int page, int pageSize)
    {
        
        try
        {
            var userId = User.GetUserId();
            var user = await _userStore.Users
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var teams = await _repository
                .Read(t => t.Workers!.Contains(user!))
                .Include(t => t.Folders)!
                .ThenInclude(f => f.Keys)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();

            var count = teams.Count;
            
            var pagedList = new PagedList<Team>(page, pageSize, count, teams);

            return Operation<PagedList<Team>>.Result(pagedList);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Operation<PagedList<Team>>.Error(e.Message);
        }
    }
}