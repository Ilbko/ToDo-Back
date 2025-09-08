using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ToDo.DAL.Data;
using ToDo.DAL.Repositories.Interfaces.Base;

namespace ToDo.DAL.Repositories.Realizations.Base;

internal class RepositoryBase<T> : IRepositoryBase<T> 
    where T : class
{
    protected readonly ToDoDbContext _dbContext;

    protected RepositoryBase(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var tmp = await _dbContext.Set<T>().AddAsync(entity);
        return tmp.Entity;
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Options.QueryOptions<T>? queryOptions = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (queryOptions != null)
        {
            query = ApplyInclude(query, queryOptions.Include);
            query = ApplyFilter(query, queryOptions.Filter);
            query = ApplyOrdering(query, queryOptions.OrderByASC, queryOptions.OrderByDESC);
            query = ApplyPagination(query, queryOptions.Offset, queryOptions.Limit);
            query = ApplySelector(query, queryOptions.Selector);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(Options.QueryOptions<T>? queryOptions = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (queryOptions != null)
        {
            query = ApplyInclude(query, queryOptions.Include);
            query = ApplyFilter(query, queryOptions.Filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public EntityEntry<T> Update(T entity)
    {
        return _dbContext.Set<T>().Update(entity);
    }

    private static IQueryable<T> ApplyFilter(IQueryable<T> query, Expression<Func<T, bool>>? filter)
    {
        return filter is not null ? query.Where(filter) : query;
    }

    private static IQueryable<T> ApplyInclude(IQueryable<T> query, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        return include is not null ? include(query) : query;
    }

    private static IQueryable<T> ApplyOrdering(
        IQueryable<T> query,
        Expression<Func<T, object>>? orderByASC,
        Expression<Func<T, object>>? orderByDESC)
    {
        if (orderByASC != null)
        {
            return query.OrderBy(orderByASC);
        }

        if (orderByDESC != null)
        {
            return query.OrderByDescending(orderByDESC);
        }

        return query;
    }

    private static IQueryable<T> ApplySelector(IQueryable<T> query, Expression<Func<T, T>>? selector)
    {
        return selector != null ? query.Select(selector) : query;
    }

    private static IQueryable<T> ApplyPagination(IQueryable<T> query, int offset, int limit)
    {
        if (offset > 0)
        {
            query = query.Skip(offset);
        }

        if (limit > 0)
        {
            query = query.Take(limit);
        }

        return query;
    }
}
