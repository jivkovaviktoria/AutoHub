using System.Linq.Expressions;

namespace AutoHub.Utilities;

public static class Extensions
{
    public static void AddException(this OperationResult operationResult, Exception exception)
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));
        if (exception is null) return;

        var error = new Error { Message = exception.Message };
        operationResult.AddError(error);
    }
    
    public static TOperationResult AppendErrors<TOperationResult>(this TOperationResult principal, OperationResult other)
        where TOperationResult : OperationResult
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal));

        foreach (var error in other.Errors) principal.AddError(error);
        return principal;
    }

    public static OperationResult<TData> WithData<TData>(this OperationResult<TData> operationResult, TData data)
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));

        operationResult.Data = data;
        return operationResult;
    }

    public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> collection,
        IEnumerable<Expression<Func<TEntity, bool>>> filters)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (filters is null) return collection;
        
        foreach (var filter in filters)
            collection = collection.Where(filter);

        return collection;
    }
    
    public static IEnumerable<T> ConcatenateWith<T>(this IEnumerable<T> a, T b)
    {
        foreach (var element in a) yield return element;
        yield return b;
    }
}