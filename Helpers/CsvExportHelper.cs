using System.Text;
using System.Reflection;

namespace BlogicCRM.Helpers
{
    public static class CsvExportHelper
    {
        public static byte[] GenerateCsv<T>(IEnumerable<T> data)
        {
            var builder = new StringBuilder();

            var properties = typeof(T).GetProperties();

            builder.AppendLine(string.Join(";", properties.Select(p => p.Name)));

            foreach (var item in data)
            {
               
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
                builder.AppendLine(string.Join(";", values));
            }

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            return new byte[] { 0xEF, 0xBB, 0xBF }.Concat(bytes).ToArray();
        }
    }
}