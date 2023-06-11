using AutoHub.Utilities;

namespace AutoHub.Tests.OperationResultTests;

public class OperationResultTests
{
    [Fact]
    public void OperationResultShouldBeInitiallySuccessful()
    {
        var operationResult = new OperationResult();

        Assert.True(operationResult.IsSuccessful);
        Assert.Equal(0, operationResult.Errors.Count);
    }

    [Fact]
    public void OperationResultWithErrorsShouldBeUnsuccessful()
    {
        var operationResult = new OperationResult();

        var result = operationResult.AddError(new Error());

        Assert.True(result);
        Assert.False(operationResult.IsSuccessful);
        Assert.Equal(1, operationResult.Errors.Count);
    }

    [Fact]
    public void AddingNullErrorShouldBeFalse()
    {
        var operationResult = new OperationResult();

        var result = operationResult.AddError(null);
        
        Assert.False(result);
        Assert.True(operationResult.IsSuccessful);
        Assert.Equal(0, operationResult.Errors.Count);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void OperationResultShouldStoreDataCorectly(int data)
    {
        var operationResult = new OperationResult<int>(){Data = data};
        Assert.Equal(operationResult.Data, data);
    }
}