using System.Linq.Expressions;
using System.Transactions;

namespace Microsoft.EntityFrameworkCore;

public static class EntityFrameworkExtenstions
{
    private static TransactionScope CreateTrancationAsync()
    {
        return new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions()
                                {
                                    IsolationLevel = IsolationLevel.ReadUncommitted
                                },
                                TransactionScopeAsyncFlowOption.Enabled);
    }

    private static TransactionScope CreateTrancation()
    {
        return new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions()
                                {
                                    IsolationLevel = IsolationLevel.ReadUncommitted
                                });
    }


    public static List<T> ToListNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
    {
        List<T> result = default;
        using var scope = CreateTrancation();

        if (expression is object)
        {
            query = query.Where(expression);
        }
        result = query.ToList();
        scope.Complete();

        return result;
    }

    public static async Task<List<T>> ToListNoLockAsync<T>(this IQueryable<T> query, 
        CancellationToken cancellationToken = default, 
        Expression<Func<T, bool>> expression = null)
    {
        List<T> result = default;

        using var scope = CreateTrancationAsync();

        if (expression is object)
        {
            query = query.Where(expression);
        }

        result = await query.ToListAsync(cancellationToken);
        scope.Complete();

        return result;
    }

    public static T FirstOrDefaultNoLock<T>(this IQueryable<T> query, 
        Expression<Func<T, bool>> expression = null)
    {
        using var scope = CreateTrancation();

        if (expression is object)
        {
            query = query.Where(expression);
        }

        T result = query.FirstOrDefault();
        scope.Complete();

        return result;
    }

    public static async Task<T> FirstOrDefaultNoLockAsync<T>(this IQueryable<T> query, 
        CancellationToken cancellationToken = default, 
        Expression<Func<T, bool>> expression = null)
    {
        using var scope = CreateTrancationAsync();

        if (expression is object)
        {
            query = query.Where(expression);
        }

        T result = await query.FirstOrDefaultAsync(cancellationToken);
        scope.Complete();

        return result;
    }

    public static int CountNoLock<T>(this IQueryable<T> query, 
        Expression<Func<T, bool>> expression = null)
    {
        using var scope = CreateTrancation();

        if (expression is object)
        {
            query = query.Where(expression);
        }

        int count = query.Count();
        scope.Complete();

        return count;
    }

    public static async Task<int> CountNoLockAsync<T>(this IQueryable<T> query, 
        CancellationToken cancellationToken = default, 
        Expression<Func<T, bool>> expression = null)
    {
        using var scope = CreateTrancationAsync();

        if (expression is object)
        {
            query = query.Where(expression);
        }

        int count = await query.CountAsync(cancellationToken);
        scope.Complete();

        return count;
    }

}
