using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.Repositories;
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
            return Operation<bool>.Error(e.Message);
        }
    }
}