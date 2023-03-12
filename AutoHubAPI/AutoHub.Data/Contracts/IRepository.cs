namespace AutoHub.Data.Contracts;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity> Get(Guid id);
    Task<ICollection<TEntity>> GetAll();
    Task<TEntity> Add(TEntity entity);
    Task<IOrderedEnumerable<TEntity>> OrderCars(string orderBy, string direction);
}