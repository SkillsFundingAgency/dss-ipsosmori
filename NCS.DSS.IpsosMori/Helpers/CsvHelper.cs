using System.Data;
using System.Text;

namespace NCS.DSS.IpsosMori.Helpers
{
    public class CsvHelper : ICsvHelper
    {
        public string ConvertDataTableToCsvAsString(DataTable dataTable)
        {

            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            var fileContent = new StringBuilder();

            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col + ",");
            }

            fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (var rowValue in dr.ItemArray)
                {
                    fileContent.Append("\"" + rowValue + "\",");
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            return fileContent.ToString();
        }
    }
}