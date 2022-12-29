using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Monirujjaman.Data.Models;
using Monirujjaman.Data.Paging;

namespace Monirujjaman.Data.Contracts;

public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : IComparable<TKey>
{
    Task<IPaginate<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default);

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TResult>, IOrderedQueryable<TResult>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TResult, bool>>? predicate = null,
        Func<IQueryable<TResult>, IOrderedQueryable<TResult>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<IPaginate<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        string? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default);

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null, string? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;
    
    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        string? predicate = null, string? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<IPaginate<TEntity>> GetPagedListAsync(IList<FilterColumnModel>? predicate = null,
        IList<SortOrderModel>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default);

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        IList<FilterColumnModel>? predicate = null, IList<SortOrderModel>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null, IList<SortOrderModel>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<IPaginate<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        IList<SortOrderModel>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 1,
        int size = 10,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default);
    
    Task<IPaginate<TEntity>> GetPagedListAsync(SearchRequestModel searchRequest,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default);

    Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        SearchRequestModel searchRequest,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool spiltQuery = false,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(TKey key, CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task DeleteAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TResult>> GetAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true, CancellationToken cancellationToken = default);

    Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true);

    Task<(IReadOnlyList<TResult> data, IDictionary<string, dynamic> output)> QueryWithStoredProcedureAsync<TResult>(
        string storedProcedureName,
        IDictionary<string, object?>? inputParameters,
        IDictionary<string, Type>? outputParameters);

    Task ExecuteSqlAsync(string sql, object[] parameters, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TResult>> ExecuteSqlAsync<TResult>(string sql, object[] parameters,
        CancellationToken cancellationToken = default);

    IQueryable<TEntity> GetQueryable();
}