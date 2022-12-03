using CleanArchitecture.Data.Enums;

namespace Monirujjaman.Data.Models;

public class FilterColumnModel
{
    public string ColumnName { get; set; } = default!;

    public OperatorType Operator { get; set; }

    public string Value { get; set; } = default!;
}