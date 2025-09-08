using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ToDo.DAL.Repositories.Options;

internal class QueryOptions<T>
{
    public Expression<Func<T, bool>>? Filter { get; set; }
    public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    public Expression<Func<T, object>>? OrderByASC { get; set; }
    public Expression<Func<T, object>>? OrderByDESC { get; set; }
    public Expression<Func<T, T>>? Selector { get; set; }
}
