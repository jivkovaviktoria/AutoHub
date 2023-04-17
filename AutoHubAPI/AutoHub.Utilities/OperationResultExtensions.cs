namespace AutoHub.Utilities;

public static class OperationResultExtensions
{
    public static void AddException(this OperationResult operationResult, Exception? exception)
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
}