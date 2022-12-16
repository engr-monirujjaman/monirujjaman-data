using Monirujjaman.Data.Enums;

namespace Monirujjaman.Data.Models;

public class SortOrderModel
{
    public string? SortBy { get; set; }

    public SortOrderType Order { get; set; }
}