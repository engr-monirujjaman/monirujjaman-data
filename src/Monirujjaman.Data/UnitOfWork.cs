using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Monirujjaman.Data.Contracts;

namespace Monirujjaman.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private TContext _dbContext;
    private Dictionary<Type, object>? _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext? dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        return _transaction.CommitAsync(cancellationToken);
    }

    public Task RoleBackTransactionAsync(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        return _transaction.RollbackAsync(cancellationToken);
    }

    public async Task ExecutionStrategyAsync(ILogger logger, string functionName,
        Func<IDbContextTransaction, Task> operation, CancellationToken cancellationToken = default)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        
        await strategy.ExecuteAsync(async () =>
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await operation(transaction);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(e, "Operation Failed On Execute {FunctionName}: {Message}", functionName, e.Message);
            }
        });
    }

    public async Task ExecuteInTransactionAsync(ILogger logger, string functionName, Func<DbContext, CancellationToken, Task> operation,
        Func<DbContext, CancellationToken, Task<bool>> verify, CancellationToken cancellationToken = default)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        try
        {
            await strategy.ExecuteInTransactionAsync(_dbContext, operation, verify, IsolationLevel.Serializable, cancellationToken);
            _dbContext.ChangeTracker.AcceptAllChanges();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Operation Failed On Execute {FunctionName}: {Message}", functionName, e.Message);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default,
        bool acceptAllChangesOnSuccess = true)
    {
        await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class, IEntity<TKey> where TKey : IComparable<TKey>
    {
        _repositories ??= new Dictionary<Type, object>();

        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type) is false)
            _repositories[type] = new Repository<TEntity, TKey, TContext>(_dbContext);

        return (_repositories[type] as IRepository<TEntity, TKey>)!;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
            _transaction?.Dispose();

            if (_repositories is not null) _repositories.Clear();
        }

        _dbContext = null!;
        _transaction = null;
    }

    private ValueTask DisposeAsyncCore()
    {
        _dbContext.DisposeAsync().ConfigureAwait(false);
        _transaction?.DisposeAsync().ConfigureAwait(false);

        _dbContext = null!;
        _transaction = null;

        return ValueTask.CompletedTask;
    }
}