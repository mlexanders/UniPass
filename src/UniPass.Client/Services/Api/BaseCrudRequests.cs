using System.Net.Http.Json;
using UniPass.Client.Utils;
using UniPass.Domain.Application;
using UniPass.Domain.Base;
using UniPass.Infrastructure;
using UniPass.Infrastructure.Repositories;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.Client.Services.Api;

public class BaseCrudRequests<TEntity, TKey> : ICrud<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected readonly HttpClient Client;
        protected readonly string _basePath;

        public BaseCrudRequests(IHttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.CreateClient(AppData.AppName);
            _basePath = Client.BaseAddress?.ToString();
        }

        public async Task<Operation<TEntity>> Create(TEntity entity)
        {
            var response = await Client.PostAsJsonAsync( $"{_basePath}/{typeof(TEntity).Name}", entity);
            return await response.GetResult<Operation<TEntity>>();
        }

        public async Task<Operation<OperationInfo>> Create(List<TEntity> entities)
        {
            var response = await Client.PostAsJsonAsync( $"{_basePath}/{typeof(TEntity).Name}/many", entities);
            return await response.GetResult<Operation<OperationInfo>>();
        }
        
        public async Task<Operation<OperationInfo>> Delete(TKey key)
        {
            var response = await Client.DeleteAsync($"{_basePath}/{key}") ?? throw new NotImplementedException();
            return await response.GetResult<Operation<OperationInfo>>();
        }

        public async Task<Operation<PagedList<TEntity>>> Read(int page, int pageSize)
        {
            var response = await Client.GetAsync( $"{_basePath}/{typeof(TEntity).Name}/{page}/{pageSize}") ?? throw new NotImplementedException();
            return await response.GetResult<Operation<PagedList<TEntity>>>();
        }

        public async Task<Operation<TEntity>> ReadFirst(TKey key, string includes = null)
        {
            var response = await Client.GetAsync($"{_basePath}/{typeof(TEntity).Name}/{key}?includes={includes}") ?? throw new NotImplementedException();
            return await response.GetResult<Operation<TEntity>>();
        }
        
        public async Task<Operation<TEntity>> Update(TEntity entity)
        {
            var response = await Client.PutAsJsonAsync($"{_basePath}/{typeof(TEntity).Name}", entity) ?? throw new NotImplementedException();
            return await response.GetResult<Operation<TEntity>>();
        }
    }