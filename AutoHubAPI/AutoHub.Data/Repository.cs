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
}