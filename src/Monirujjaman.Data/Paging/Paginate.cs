namespace Monirujjaman.Data.Paging;

public sealed class Paginate<T> : IPaginate<T>
{
    public Paginate(IEnumerable<T> source, int index, int size, int total, int from = 1)
    {
        var enumerable = source as T[] ?? source.ToArray();

        if (from > index)
            throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");

        if (source is IQueryable<T> queryable)
        {
            Index = index;
            Size = size;
            From = from;
            TotalFiltered = queryable.Count();
            Total = total;
            Pages = (int)Math.Ceiling(TotalFiltered / (double)Size);
            Items = queryable.Skip((Index - From) * Size).Take(Size).ToList();
        }
        else
        {
            Index = index;
            Size = size;
            From = from;
            TotalFiltered = enumerable.Length;
            Total = total;
            Pages = (int)Math.Ceiling(TotalFiltered / (double)Size);
            Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
        }
    }

    public Paginate()
    {
        Items = Array.Empty<T>();
    }

    public int Index { get; init; }

    public int Size { get; init; }

    public int TotalFiltered { get; init; }

    public int Total { get; init; }

    public int Pages { get; init; }

    public int From { get; init; }

    public IReadOnlyCollection<T> Items { get; init; }
    public bool HasPrevious => Index - From > 0;
    public bool HasNext => Index - From + 1 < Pages;
}

public class Paginate<TSource, TResult> : IPaginate<TResult>
{
    public Paginate(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter,
        int index, int size, int total, int from = 1)
    {
        var enumerable = source as TSource[] ?? source.ToArray();

        if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

        if (source is IQueryable<TSource> queryable)
        {
            Index = index;
            Size = size;
            From = from;
            TotalFiltered = queryable.Count();
            Total = total;
            Pages = (int)Math.Ceiling(TotalFiltered / (double)Size);
            var items = queryable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
        }
        else
        {
            Index = index;
            Size = size;
            From = from;
            TotalFiltered = enumerable.Length;
            Pages = (int)Math.Ceiling(TotalFiltered / (double)Size);
            var items = enumerable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
        }
    }

    public Paginate(IPaginate<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        Index = source.Index;
        Size = source.Size;
        From = source.From;
        TotalFiltered = source.TotalFiltered;
        Total = source.Total;
        Pages = source.Pages;
        Items = new List<TResult>(converter(source.Items));
    }

    public Paginate()
    {
        Items = Array.Empty<TResult>();
    }

    public int Index { get; init; }

    public int Size { get; init; }

    public int TotalFiltered { get; init; }

    public int Total { get; init; }

    public int Pages { get; init; }

    public int From { get; init; }

    public IReadOnlyCollection<TResult> Items { get; init; }

    public bool HasPrevious => Index - From > 0;

    public bool HasNext => Index - From + 1 < Pages;
}

public static class Paginate
{
    public static IPaginate<T> Empty<T>()
    {
        return new Paginate<T>();
    }

    public static IPaginate<TResult> From<TResult, TSource>(IPaginate<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        return new Paginate<TSource, TResult>(source, converter);
    }
}