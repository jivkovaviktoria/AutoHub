using AutoHub.Data.Contracts;

namespace AutoHub.Data.Models;

public class BaseEntity : IEntity
{
    public Guid Id { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
}