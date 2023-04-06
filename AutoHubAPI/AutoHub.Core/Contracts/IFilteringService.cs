using AutoHub.Core.FilterDefinitions;
using AutoHub.Data.Contracts;
using AutoHub.Utilities;

namespace AutoHub.Core.Contracts;

public interface IFilteringService<TEntity>
    where TEntity : class, IEntity
{
    Task<OperationResult<IEnumerable<TEntity>>> OrderBy(OrderDefinition orderDefinition);
}