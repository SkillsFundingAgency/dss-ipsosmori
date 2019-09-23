using System.Data;
using NCS.DSS.IpsosMori.DB;
using NCS.DSS.IpsosMori.Helpers;

namespace NCS.DSS.IpsosMori.SatisfactionExtractHttpTrigger.Service
{
    public class SatisfactionExtractService : ISatisfactionExtractService
    {
        private readonly IAzureSqlDbProvider _azureSqlDbProvider;
        private readonly IFtpHelper _ftpHelper;
        private readonly ICsvHelper _csvHelper;

        public SatisfactionExtractService(IAzureSqlDbProvider azureSqlDbProvider, IFtpHelper ftpHelper, ICsvHelper csvHelper)
        {
            _azureSqlDbProvider = azureSqlDbProvider;
            _ftpHelper = ftpHelper;
            _csvHelper = csvHelper;
        }

        public DataTable GetSatisfactionExtractDataTable()
        {
            return _azureSqlDbProvider.GetSatisfactionExtract();
        }

        public string ConvertDataTableToCsvAsString(DataTable dataTable)
        {
           return _csvHelper.ConvertDataTableToCsvAsString(dataTable);
        }

        public void UploadCsvDataToFtp(string stream, string filePath)
        {
            _ftpHelper.UploadDataToFtp(stream, filePath);
        }

    }
}
