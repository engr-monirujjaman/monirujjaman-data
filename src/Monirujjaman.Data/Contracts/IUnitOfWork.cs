using Z.BulkOperations;

namespace Monirujjaman.Data.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task SaveAsync();
    Task SaveAsync(Action<BulkOperation> options, CancellationToken cancellationToken = default);
}