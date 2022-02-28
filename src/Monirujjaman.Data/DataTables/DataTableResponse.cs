namespace Monirujjaman.Data.DataTables
{
    public class DataTableResponse<T>
    {
        public DataTableResponse(int draw, int recordsTotal, int recordsFiltered, IEnumerable<T> data, string error)
        {
            Draw = draw;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            Data = data.ToArray();
            Error = error;
        }

        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public T[] Data { get; set; }
        public string Error { get; set; } = string.Empty;

        public static DataTableResponse<T> GetEmptyResponse()
        {
            return new DataTableResponse<T>(0, 0, 0, new List<T>(), string.Empty);
        }
    }
}