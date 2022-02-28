using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Monirujjaman.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Monirujjaman.Data.Paging;
using Microsoft.Data.SqlClient;

namespace Monirujjaman.Data;

public class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey, TContext>
    where TEntity : class, IEntity<TKey> where TContext : DbContext where TKey : IComparable
{
    protected Repository(TContext context)
    {
        Context = context;
    }

    public TContext Context { get; }

    public async Task<IPaginate<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0,
        int size = 10, bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken)
            : await query.ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        string? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0,
        int size = 10, bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await query.OrderBy(orderBy).ToPaginateAsync(index, size, 0, cancellationToken)
            : await query.ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>>? selector,
        Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0, int size = 10, bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await orderBy(query).Select(selector!).ToPaginateAsync(index, size, 0, cancellationToken)
            : await query.Select(selector!).ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IPaginate<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>>? selector,
        Expression<Func<TEntity, bool>>? predicate = null, string? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0, int size = 10, bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await query.OrderBy(orderBy).Select(selector!).ToPaginateAsync(index, size, 0, cancellationToken)
            : await query.Select(selector!).ToPaginateAsync(index, size, 0, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, TResult>>? selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await orderBy(query).Select(selector!).ToListAsync(cancellationToken)
            : await query.Select(selector!).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return orderBy is not null
            ? await orderBy(query).FirstOrDefaultAsync(cancellationToken)
            : await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>>? selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        if (disableTracking) query.AsNoTracking();
        if (include is not null) query = include(query);
        if (predicate is not null) query = query.Where(predicate);
        return (orderBy is not null
            ? await orderBy(query).Select(selector!).FirstOrDefaultAsync()
            : await query.Select(selector!).FirstOrDefaultAsync())!;
    }

    public async Task<TEntity?>? FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().FindAsync(keyValues, cancellationToken);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return predicate is not null
            ? await Context.Set<TEntity>().CountAsync(predicate, cancellationToken)
            : await Context.Set<TEntity>().CountAsync(cancellationToken);
    }

    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().AnyAsync(predicate, cancellationToken);

    public async Task<(IEnumerable<TResult> data, IDictionary<string, dynamic> output)>
        QueryWithStoredProcedureAsync<TResult>(
            string storedProcedureName,
            IDictionary<string, object?>? inputParameters,
            IDictionary<string, Type>? outputParameters)
    {
        var command = CreateCommand(storedProcedureName, inputParameters, outputParameters);

        if (command.Connection!.State.Equals(ConnectionState.Closed))
            await command.Connection.OpenAsync();

        List<TResult> data;

        try
        {
            data = (await ExecuteQueryAsync<TResult>(command)).ToList();
        }
        finally
        {
            if (command.Connection.State.Equals(ConnectionState.Closed) is false)
                await command.Connection.CloseAsync();
        }

        return (data, GetOutputParameterValues(command, outputParameters));
    }

    private DbCommand CreateCommand(string storedProcedureName,
        IDictionary<string, object?>? inputParameters,
        IDictionary<string, Type>? outputParameters)
    {
        var command = Context.Database.GetDbConnection().CreateCommand();
        command.CommandText = storedProcedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 300;

        if (inputParameters is not null)
        {
            foreach (var parameter in inputParameters)
                command.Parameters.Add(CreateInputDbParameter(parameter.Key, parameter.Value));
        }

        if (outputParameters is not null)
        {
            foreach (var parameter in outputParameters)
                command.Parameters.Add(CreateOutputDbParameter(parameter.Key, parameter.Value));
        }

        return command;
    }

    private DbParameter CreateInputDbParameter(string name, object? value)
    {
        return new SqlParameter
        {
            ParameterName = name,
            Value = value != null && GetDbType(value.GetType()) == SqlDbType.DateTime
                                  && Convert.ToDateTime(value) < SqlDateTime.MinValue.Value
                ? SqlDateTime.MinValue.Value
                : value,
            Direction = ParameterDirection.Input
        };
    }

    private DbParameter CreateOutputDbParameter(string name, Type type)
    {
        return new SqlParameter(name, GetDbType(type))
        {
            Direction = ParameterDirection.Output
        };
    }

    private SqlDbType GetDbType(Type type) => type switch
    {
        { } when type == typeof(int) || type == typeof(uint) ||
                 type == typeof(short) || type == typeof(ushort) => SqlDbType.Int,
        { } when type == typeof(double) || type == typeof(decimal) => SqlDbType.Decimal,
        { } when type == typeof(long) || type == typeof(ulong) => SqlDbType.BigInt,
        { } when type == typeof(string) => SqlDbType.NVarChar,
        { } when type == typeof(DateTime) => SqlDbType.DateTime,
        { } when type == typeof(bool) => SqlDbType.Bit,
        { } when type == typeof(Guid) => SqlDbType.UniqueIdentifier,
        { } when type == typeof(Enum) => SqlDbType.Int,
        _ => SqlDbType.NVarChar
    };

    private IDictionary<string, dynamic> GetOutputParameterValues(
        DbCommand command, IDictionary<string, Type>? outputParameters)
    {
        var result = new Dictionary<string, dynamic>();
        if (outputParameters is not null)
            foreach (var parameter in outputParameters)
                result.Add(parameter.Key, command.Parameters[parameter.Key].Value!);
        return result;
    }

    private async Task<IList<TResult>> ExecuteQueryAsync<TResult>(DbCommand command)
    {
        var reader = await command.ExecuteReaderAsync();
        var result = new List<TResult>();

        while (await reader.ReadAsync())
        {
            var instance = Activator.CreateInstance<TResult>();
            if (instance is not null)
            {
                instance.GetType().GetProperties().ToList().ForEach(x =>
                {
                    x.SetValue(instance, Convert.ChangeType(reader.GetValue(x.Name), x.PropertyType, null));
                });
                result.Add(instance);
            }
        }

        return result;
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await InsertAsync(new[] {entity}, cancellationToken);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Context.BulkInsertAsync(entities, x => { x.InsertIfNotExists = true; }, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(new[] {entity}, cancellationToken);
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Context.BulkUpdateAsync(entities, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(new[] {entity}, cancellationToken);
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Context.BulkDeleteAsync(entities, cancellationToken);
    }
}