using AutoHub.Core.Contracts;
using AutoHub.Data.Contracts;
using AutoHub.Utilities;

namespace AutoHub.Core.Services;

public class Service<TEntity> : IService<TEntity>
    where TEntity : class, IEntity
{
    private readonly IRepository<TEntity> _repository;

    public Service(IRepository<TEntity> repository)
    {
        this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<OperationResult<bool>> AnyAsync(Guid id)
    {
        var operationResult = new OperationResult<bool>();

        var result = await this._repository.AnyAsync(id);
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        return operationResult.WithData(result.Data);
    }

    public async Task<OperationResult<bool>> ExistAsync(Guid id)
    {
        var operationResult = new OperationResult<bool>();

        var result = await this._repository.AnyAsync(id);
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        return operationResult.WithData(result.Data);
    }

    public async Task<OperationResult<TEntity>> GetAsync(Guid id)
    {
        var operationResult = new OperationResult<TEntity>();
        
        var result = await this._repository.GetAsync(id);
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        return operationResult.WithData(result.Data);
    }

    public async Task<OperationResult<IEnumerable<TEntity>>> GetManyAsync()
    {
        var operationResult = new OperationResult<IEnumerable<TEntity>>();

        var result = await this._repository.GetManyAsync();
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        return operationResult.WithData(result.Data);
    }

    public async Task<OperationResult<TEntity>> CreateAsync(TEntity entity)
    {
        var operationResult = new OperationResult<TEntity>();

        var result = await this._repository.CreateAsync(entity);
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        return operationResult.WithData(entity);
    }
}