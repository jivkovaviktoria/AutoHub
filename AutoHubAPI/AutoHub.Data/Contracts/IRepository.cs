using AutoHub.Utilities;

namespace AutoHub.Data.Contracts;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<OperationResult<bool>> AnyAsync(Guid id);
    Task<OperationResult<TEntity>> GetAsync(Guid id);
    Task<OperationResult<IEnumerable<TEntity>>> GetManyAsync();
    Task<OperationResult> CreateAsync(TEntity entity);
    Task<OperationResult> DeleteAsync(TEntity entity);
}