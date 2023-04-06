using AutoHub.Data.Contracts;
using AutoHub.Utilities;

namespace AutoHub.Core.Contracts;

public interface IService<TEntity>
    where TEntity : class, IEntity
{
    Task<OperationResult<TEntity>> GetAsync(Guid id);
    Task<OperationResult<IEnumerable<TEntity>>> GetManyAsync();
    Task<OperationResult<TEntity>> CreateAsync(TEntity entity);
    Task<OperationResult<TEntity>> DeleteAsync(TEntity entity);
}