using System.Data;

namespace NCS.DSS.IpsosMori.SatisfactionExtractHttpTrigger.Service
{
    public interface ISatisfactionExtractService
    {
        DataTable GetSatisfactionExtractDataTable();
        string ConvertDataTableToCsvAsString(DataTable dataTable);
        void UploadCsvDataToFtp(string stream, string filePath);
    }
}