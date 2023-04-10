using System.Reflection;
using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Data.Contracts;
using AutoHub.Utilities;

namespace AutoHub.Core.Services;

public class FilteringService<TEntity> : IFilteringService<TEntity>
    where TEntity : class, IEntity
{
    private readonly IService<TEntity> _service;

    public FilteringService(IService<TEntity> service)
    {
        this._service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<OperationResult<IEnumerable<TEntity>>> OrderBy(OrderDefinition orderDefinition)
    {
        var operationResult = new OperationResult<IEnumerable<TEntity>>();
        
        var result = await this._service.GetManyAsync();
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);

        var entities = result.Data;

        var prop = typeof(TEntity).GetProperty(orderDefinition.Property, 
            BindingFlags.IgnoreCase | 
            BindingFlags.Public |
            BindingFlags.Instance);

       
        
        if (orderDefinition.IsAscending) entities = entities.OrderBy(x => prop.GetValue(x, null)).ToList();
        else entities = entities.OrderByDescending(x => prop.GetValue(x, null)).ToList();

        return operationResult.WithData(entities);
    }
}