using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UniPass.Domain;
using UniPass.Infrastructure.Repositories;
using UniPass.Infrastructure.Services;
using UniPass.Infrastructure.ViewModels;
using UniPass.WebApi.Repositories;
using UniPass.WebApi.Utils;

namespace UniPass.WebApi.Controllers;

[Authorize]
public class KeyController : CrudController<Key, Guid>
{
    private readonly FoldersRepository _foldersRepository;

    public KeyController(KeysRepository repository, FoldersRepository foldersRepository,
        UniPassBaseLogger<CrudController<Key, Guid>> logger) : base(
        repository, logger)
    {
        _foldersRepository = foldersRepository;
    }

    public override async Task<Operation<Key>> Create(Key entity)
    {
        try
        {
            if (entity is null) throw new UniPassApiException("Невалидный объект");

            var creatorUserId = User.GetUserId();


            var targetFolder = await _foldersRepository
                .Read(f => f.Id.Equals(entity.FolderId) && f.OwnerId.Equals(creatorUserId))
                .FirstOrDefaultAsync();

            if (targetFolder is null) throw new UniPassApiException("Не найдена папка для сохранения");

            var result = await Repository.Create(entity);
            return Operation<Key>.Result(result, "Запись успешно добавлена");
        }
        catch (UniPassApiException e)
        {
            return Operation<Key>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return Operation<Key>.Error(e.Message);
        }
    }

    public override async Task<Operation<OperationInfo>> Create(List<Key> entities)
    {
        try
        {
            if (entities is null) throw new UniPassApiException("Невалидный объект");

            var creatorUserId = User.GetUserId();

            var targetFoldersId = entities
                .Select(k => k.FolderId)
                .Distinct().ToList();

            var folders = await _foldersRepository
                .Read(f => targetFoldersId.Contains(f.Id) && f.OwnerId.Equals(creatorUserId))
                .Distinct()
                .ToListAsync();

            if (folders is null) throw new UniPassApiException("Не найдена папка для сохранения");
            if (folders.Count != targetFoldersId.Count) throw new UniPassApiException("Не все папки найдены для сохранения");

            return await base.Create(entities);
        }
        catch (UniPassApiException e)
        {
            return Operation<OperationInfo>.Error(e.Message);
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return Operation<OperationInfo>.Error(e.Message);
        }
    }
}