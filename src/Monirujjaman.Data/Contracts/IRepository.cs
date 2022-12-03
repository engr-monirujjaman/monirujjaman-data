using Microsoft.EntityFrameworkCore;

namespace Monirujjaman.Data.Contracts;

public interface IRepository<TEntity, in TKey, out TContext> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> where TContext : DbContext where TKey : IComparable<TKey>
{
    protected TContext DbContext { get; }
    
    protected DbSet<TEntity> DbSet { get; }
}