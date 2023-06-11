using AutoHub.Core.Contracts;
using AutoHub.Data.Contracts;
using AutoHub.Data.Models;
using AutoHub.Utilities;

namespace AutoHub.Core.Services;

public class ReviewService : Service<Review>, IReviewService
{
    public ReviewService(IRepository<Review> repository) : base(repository)
    { }

    public async Task<OperationResult<IEnumerable<Review>>> GetByUser(string userId)
    {
        var operationResult = new OperationResult<IEnumerable<Review>>();
        
        var result = await this._repository.GetManyAsync();
        if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        
        var userReviews = result?.Data?.Where(x => x.UserId == userId);

        return operationResult.WithData(userReviews);
    }
}