namespace Monirujjaman.Data.DatatableUtilities;

public sealed class DatatableColumn
{
    public string Data { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool Searchable { get; set; }

    public bool Orderable { get; set; }

    public DatatableSearch Search { get; set; } = new();
}