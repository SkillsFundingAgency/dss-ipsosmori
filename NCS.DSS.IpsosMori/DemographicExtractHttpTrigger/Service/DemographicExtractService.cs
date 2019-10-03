using System;
using System.Data;
using System.Globalization;
using System.Text;
using NCS.DSS.IpsosMori.DB;
using NCS.DSS.IpsosMori.Helpers;

namespace NCS.DSS.IpsosMori.DemographicExtractHttpTrigger.Service
{
    public class DemographicExtractService : IDemographicExtractService
    {
        private readonly IAzureSqlDbProvider _azureSqlDbProvider;
        private readonly IFtpHelper _ftpHelper;

        public DemographicExtractService(IAzureSqlDbProvider azureSqlDbProvider, IFtpHelper ftpHelper)
        {
            _azureSqlDbProvider = azureSqlDbProvider;
            _ftpHelper = ftpHelper;
        }

        public DataTable GetDemographicExtractDataTable()
        {
            return _azureSqlDbProvider.GetDemographicExtract();
        }


        public void UploadCsvDataToFtp(string stream, string filePath)
        {
            _ftpHelper.UploadDataToFtp(stream, filePath);
        }

        public string ConvertDataTableToCsvAsString(DataTable dataTable)
        {
            PreProcessRawQueryData(dataTable);

            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            var fileContent = new StringBuilder();

            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col + ",");
            }

            fileContent.Replace(",", Environment.NewLine, fileContent.Length - 1, 1);

            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (var rowValue in dr.ItemArray)
                {

                    if (rowValue.ToString() == string.Empty)
                    {
                        fileContent.Append(",");
                        continue;
                    }

                    fileContent.Append("\"" + rowValue + "\",");
                }

                fileContent.Replace(",", Environment.NewLine, fileContent.Length - 1, 1);
            }

            return fileContent.ToString();
        }

        private void PreProcessRawQueryData(DataTable dataTable)
        {
            dataTable.Columns[1].ReadOnly = false;

            dataTable.Columns.RemoveAt(0); 
            dataTable.Columns[0].ColumnName = DateTime.Now.AddMonths(-1).ToString("MMM-yy", CultureInfo.InvariantCulture);

            dataTable.Rows.InsertAt(dataTable.NewRow(), 0);
            dataTable.Rows[1][0] = "Age";

            dataTable.Rows.InsertAt(dataTable.NewRow(), 6);
            dataTable.Rows[6][0] = "Employment Status adult - By employment category";

            dataTable.Rows.InsertAt(dataTable.NewRow(), 14);
            dataTable.Rows[14][0] = "Gender";

            dataTable.Rows.InsertAt(dataTable.NewRow(), 20);
            dataTable.Rows[20][0] = "National Careers Helpline";
        }
    }
}