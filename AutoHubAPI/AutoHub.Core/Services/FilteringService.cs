using System.Reflection;
using AutoHub.Core.Contracts;
using AutoHub.Core.FilterDefinitions;
using AutoHub.Data.Models;
using AutoHub.Utilities;

namespace AutoHub.Core.Services;

public class FilteringService : IFilteringService<Car>
{
    private readonly IService<Car> _service;

    public FilteringService(IService<Car> service)
    {
        this._service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<OperationResult<IEnumerable<Car>>> OrderBy(OrderDefinition orderDefinition)
    {
        var operationResult = new OperationResult<IEnumerable<Car>>();

        var result = await this._service.GetManyAsync();
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);

        var entities = result.Data;

        var prop = typeof(Car).GetProperty(orderDefinition.Property,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Instance);

        if (orderDefinition.IsAscending) entities = entities.OrderBy(x => prop.GetValue(x, null)).ToList();
        else entities = entities.OrderByDescending(x => prop.GetValue(x, null)).ToList();

        return operationResult.WithData(entities);
    }

    public async Task<OperationResult<IEnumerable<Car>>> Filter(IEnumerable<Car> cars, GlobalCarFilter? filter)
    {
        if (filter != null)
        {
            var result = await filter.Filter(cars);
            return new OperationResult<IEnumerable<Car>>() { Data = result };
        }

        return new OperationResult<IEnumerable<Car>>() { Data = cars };
    }
}