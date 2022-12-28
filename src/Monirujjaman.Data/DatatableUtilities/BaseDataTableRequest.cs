using Monirujjaman.Data.Enums;
using Monirujjaman.Data.Models;

namespace Monirujjaman.Data.DatatableUtilities;

public abstract class BaseDataTableRequest
{
    public required int Start { get; set; } = 1;

    public required int Length { get; set; } = 10;

    public required int Draw { get; set; } = 0;

    public DatatableSearch Search { get; set; } = new();

    public List<DatatableOrder> Order { get; set; } = new();

    public required List<DatatableColumn> Columns { get; set; } = new();

    public virtual SearchRequestModel GetSearchRequest()
    {
        var request = new SearchRequestModel
        {
            PageIndex = Start > 0 ? Start / Length + 1 : 1,
            PageSize = Length == 0 ? 10 : Length,
            Sorts = PrepareOrdering(),
            Filters = PrepareFiltering()
        };

        return request;
    }

    public virtual List<SortOrderModel> PrepareOrdering()
    {
        var sortOrders = new List<SortOrderModel>();

        foreach (var order in Order)
        {
            var column = Columns.Count > order.Column ? Columns[order.Column] : null;

            if (column is not null)
                sortOrders.Add(new SortOrderModel
                {
                    SortBy = column.Name,
                    Order = order.Dir.ToLower() switch
                    {
                        "asc" => SortOrderType.Ascending,
                        "desc" => SortOrderType.Descending,
                        _ => SortOrderType.Ascending
                    }
                });
        }


        return sortOrders;
    }

    public abstract List<FilterColumnModel> PrepareFiltering();
}