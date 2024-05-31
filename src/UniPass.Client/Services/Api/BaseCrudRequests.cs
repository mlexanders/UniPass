using System.Net.Http.Json;
using UniPass.Client.Utils;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Contracts;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class BaseCrudRequests<TEntity, TKey> : ICrud<TEntity, TKey> where TEntity : Entity<TKey>
{
    protected readonly HttpClient Client;
    protected readonly string BasePath;
    protected readonly string EntityPath;

    public BaseCrudRequests(IHttpClientFactory httpClientFactory)
    {
        Client = httpClientFactory.CreateClient(AppData.AppName);
        BasePath = Client.BaseAddress?.ToString();
        EntityPath = $"{BasePath}/{typeof(TEntity).Name}";
    }
    
    public async Task<Operation<TEntity>> Create(TEntity entity)
    {
        var response = await Client.PostAsJsonAsync( $"{EntityPath}", entity);
        return await response.GetResult<Operation<TEntity>>();
    }

    public async Task<Operation<OperationInfo>> Create(List<TEntity> entities)
    {
        var response = await Client.PostAsJsonAsync( $"{EntityPath}/many", entities);
        return await response.GetResult<Operation<OperationInfo>>();
    }
    
    public async Task<Operation<OperationInfo>> Delete(TKey key)
    {
        var response = await Client.DeleteAsync($"{EntityPath}/{key}");
        return await response.GetResult<Operation<OperationInfo>>();
    }

    public async Task<Operation<PagedList<TEntity>>> Read(int page, int pageSize)
    {
        var response = await Client.GetAsync($"{EntityPath}/{page}/{pageSize}");
        return await response.GetResult<Operation<PagedList<TEntity>>>();
    }

    public async Task<Operation<TEntity>> ReadFirst(TKey key, string includes = null)
    {
        var response = await Client.GetAsync($"{EntityPath}/{key}?includes={includes}");
        return await response.GetResult<Operation<TEntity>>();
    }
    
    public async Task<Operation<TEntity>> Update(TEntity entity)
    {
        var response = await Client.PutAsJsonAsync($"{EntityPath}", entity);
        return await response.GetResult<Operation<TEntity>>();
    }
}