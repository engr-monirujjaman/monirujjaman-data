using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Monirujjaman.Data.Contracts;

public interface IUnitOfWork<TContext> : IDisposable, IAsyncDisposable where TContext : DbContext
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RoleBackTransactionAsync(CancellationToken cancellationToken = default);

    Task ExecutionStrategyAsync(ILogger logger, string functionName,
        Func<IDbContextTransaction, Task> operation, CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(ILogger logger, string functionName,
        Func<DbContext, CancellationToken, Task> operation,
        Func<DbContext, CancellationToken, Task<bool>> verify, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default, bool acceptAllChangesOnSuccess = true);

    IRepository<TEntity, TKey> Repository<TEntity, TKey>()
        where TEntity : class, IEntity<TKey> where TKey : IComparable<TKey>;
}