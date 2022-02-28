using System.Text.Json.Serialization;

namespace Monirujjaman.Data.DataTables
{
    public class DataTableColumn
    {
        [JsonPropertyName("data")]
        public string? Data { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }
        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }
        [JsonPropertyName("className")]
        public string? ClassName { get; set; }
        [JsonPropertyName("autoWidth")]
        public bool AutoWidth { get; set; }
        [JsonPropertyName("search")]
        public string? Search { get; set; }
    }
}