using AutoHub.Data.Contracts;
using AutoHub.Utilities;
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


    public async Task<OperationResult<bool>> AnyAsync(Guid id)
    {
        var operationResult = new OperationResult<bool>();

        try
        {
            var result = await this._context.Set<TEntity>().AnyAsync(x => x.Id == id);
            operationResult.Data = result;
        }
        catch (Exception e)
        {
            operationResult.AddException(e);
        }

        return operationResult;
    }

    public async Task<OperationResult<TEntity>> GetAsync(Guid id)
    {
        var operationResult = new OperationResult<TEntity>();

        try
        {
            var result = await this._context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            operationResult.Data = result;
        }
        catch (Exception e)
        {
            operationResult.AddException(e);
        }

        return operationResult;
    }

    public async Task<OperationResult<IEnumerable<TEntity>>> GetManyAsync()
    {
        var operationResult = new OperationResult<IEnumerable<TEntity>>();

        try
        {
            var result = await this._context.Set<TEntity>().ToListAsync();
            operationResult.Data = result;
        }
        catch (Exception e)
        {
            operationResult.AddException(e);
        }

        return operationResult;
    }

    public async Task<OperationResult> CreateAsync(TEntity entity)
    {
        var operationResult = new OperationResult();
        if (entity is null) return operationResult;

        try
        {
            await this._context.Set<TEntity>().AddAsync(entity);
            await this._context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            operationResult.AddException(e);
        }

        return operationResult;
    }

    public async Task<OperationResult> DeleteAsync(TEntity entity)
    {
        var operationResult = new OperationResult();
        if (entity is null) return operationResult;

        try
        {
            this._context.Remove(entity);
            await this._context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            operationResult.AddException(e);
        }

        return operationResult;
    }
}