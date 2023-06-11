using AutoHub.Data.Models;
using AutoHub.Utilities;

namespace AutoHub.Core.Contracts;

public interface IReviewService : IService<Review>
{
    Task<OperationResult<IEnumerable<Review>>> GetByUser(string userId);
}