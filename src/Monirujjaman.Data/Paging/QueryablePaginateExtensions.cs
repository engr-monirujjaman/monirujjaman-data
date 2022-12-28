﻿using Microsoft.EntityFrameworkCore;

namespace Monirujjaman.Data.Paging;

public static class QueryablePaginateExtensions
{
    public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size,
        int total, int from = 1, CancellationToken cancellationToken = default)
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((index - from) * size)
            .Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

        var list = new Paginate<T>
        {
            Index = index,
            Size = size,
            From = from,
            TotalFiltered = count,
            Total = total,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };

        return list;
    }

    public static async Task<IPaginate<TResult>> ToPaginateAsync<TSource, TResult>(this IQueryable<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int total, int from = 1,
        CancellationToken cancellationToken = default)
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((index - from) * size)
            .Take(size).ToArrayAsync(cancellationToken).ConfigureAwait(false);

        var list = new Paginate<TSource, TResult>
        {
            Index = index,
            Size = size,
            From = from,
            TotalFiltered = count,
            Total = total,
            Items = new List<TResult>(converter(items)),
            Pages = (int)Math.Ceiling(count / (double)size)
        };

        return list;
    }
}