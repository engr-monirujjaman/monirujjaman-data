namespace Monirujjaman.Data.Contracts;

public interface IUnitOfWork<TContext> : IDisposable, IAsyncDisposable
{
    Task SaveAsync(bool acceptedAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
}