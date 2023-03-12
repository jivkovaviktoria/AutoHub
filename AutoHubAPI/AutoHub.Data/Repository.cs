using System.ComponentModel;
using System.Reflection;
using AutoHub.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly AutoHubDbContext _context;

    public Repository(AutoHubDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<TEntity> Get(Guid id)
    {
        return await this._context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<TEntity>> GetAll()
    {
        return await this._context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        await this._context.Set<TEntity>().AddAsync(entity);
        await this._context.SaveChangesAsync();
        return entity;
    }

    public async Task<IOrderedEnumerable<TEntity>> OrderCars(string orderBy, [DefaultValue("asc")]string direction)
    {
        var cars = await this._context.Set<TEntity>().ToListAsync();

        var property = typeof(TEntity).GetProperty(orderBy,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        var ordered = direction.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? cars.OrderByDescending(x => property.GetValue(x))
            : cars.OrderBy(x => property.GetValue(x));

        return ordered;
    }
}