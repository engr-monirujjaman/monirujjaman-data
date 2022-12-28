using Monirujjaman.Data.Paging;

namespace Monirujjaman.Data.DatatableUtilities;

public sealed class DatatableResponse<T> where T : class
{
    private DatatableResponse(int draw, int recordsTotal, int recordsFiltered, IReadOnlyList<T> data, string error)
    {
        Draw = draw;
        RecordsTotal = recordsTotal;
        RecordsFiltered = recordsFiltered;
        Data = data;
        Error = error;
    }

    public int Draw { get; }
    public int RecordsTotal { get; }
    public int RecordsFiltered { get; }
    public IReadOnlyList<T> Data { get; }
    public string Error { get; }

    public static DatatableResponse<T> ToResponse(int draw, IPaginate<T> pagingData)
    {
        return new(draw, pagingData.TotalFiltered, pagingData.TotalFiltered, pagingData.Items.ToList(), string.Empty);
    }

    public static DatatableResponse<T> ToResponse(int draw, int totalData, int totalFilteredData, IReadOnlyList<T> data)
    {
        return new(draw, totalData, totalFilteredData, data, string.Empty);
    }

    public static DatatableResponse<T> Empty()
    {
        return new(0, 0, 0, new List<T>(), string.Empty);
    }
}