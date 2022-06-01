using Microsoft.EntityFrameworkCore;
using Monirujjaman.Data.Contracts;

namespace Monirujjaman.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private DbContext? _dbContext;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext ;
    }
    
    public Task SaveAsync(bool acceptedAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
    {
        return _dbContext?.SaveChangesAsync(acceptedAllChangesOnSuccess, cancellationToken)!;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    
    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        _dbContext?.Dispose();
        _dbContext = null;
    }
    
    public ValueTask DisposeAsync()
    {
        DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
        return default;
    }
    
    private async ValueTask DisposeAsyncCore()
    {
        if (_dbContext is not null)
        {
            await _dbContext.DisposeAsync().ConfigureAwait(false);
        }
    }
}