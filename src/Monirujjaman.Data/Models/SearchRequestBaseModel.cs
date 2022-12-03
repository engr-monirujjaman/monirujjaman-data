namespace Monirujjaman.Data.Models;

public class SearchRequestBaseModel
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    
    public List<SortOrderModel> Sorts { get; set; } = new();

    public List<FilterColumnModel> Filters { get; set; } = new();
}