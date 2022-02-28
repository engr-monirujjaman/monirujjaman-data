using Microsoft.EntityFrameworkCore;
using Monirujjaman.Data.Contracts;
using Z.BulkOperations;

namespace Monirujjaman.Data;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync()
    {
        await _dbContext.BulkSaveChangesAsync();
    }

    public async Task SaveAsync(Action<BulkOperation> options, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkSaveChangesAsync(options, cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}