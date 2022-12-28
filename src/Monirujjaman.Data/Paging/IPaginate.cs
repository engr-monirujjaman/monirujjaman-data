namespace Monirujjaman.Data.Paging;

public interface IPaginate<out T>
{
    int From { get; }

    int Index { get; }

    int Size { get; }

    int TotalFiltered { get; }

    int Total { get; }

    int Pages { get; }

    IReadOnlyCollection<T> Items { get; }

    bool HasPrevious { get; }

    bool HasNext { get; }
}