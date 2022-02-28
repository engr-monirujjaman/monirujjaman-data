using System.Text;
using Microsoft.AspNetCore.Http;

namespace Monirujjaman.Data.DataTables
{
    public class DataTableRequest
    {
        private readonly HttpRequest _request;
        private readonly string[] _columns;

        public DataTableRequest(HttpRequest request, string[] columns)
        {
            _request = request;
            _columns = columns;
        }

        public int Draw => Convert.ToInt32(_request.Form["draw"].FirstOrDefault());
        public int Start => Convert.ToInt32(_request.Form["start"].FirstOrDefault()) / Length;
        public int Length => Convert.ToInt32(_request.Form["length"].FirstOrDefault());
        public string Order => GetColumnOrders();
        public string Search => _request.Form["search[value]"].FirstOrDefault()!;

        private string GetColumnOrders()
        {
            var orderString = new StringBuilder();
            for (var i = 0; i < _columns.Length; i++)
            {
                if (_request.Form.ContainsKey($"order[{i}][column]"))
                {
                    var columnIndex = Convert.ToInt32(_request.Form[$"order[{i}][column]"].FirstOrDefault()!);
                    var direction = _request.Form[$"order[{i}][dir]"].FirstOrDefault();
                    var column = $"{_columns[columnIndex]} {(direction == "asc" ? "asc" : "desc")}, ";
                    orderString.Append(column);
                }
            }

            return orderString.Length > 2 ? orderString.Remove(orderString.Length - 2, 2).ToString() : string.Empty;
        }
    }
}