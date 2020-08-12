using System.Data;

namespace NCS.DSS.IpsosMori.DemographicExtractHttpTrigger.Service
{
    public interface IDemographicExtractService
    {
        DataTable GetDemographicExtractDataTable();
        string ConvertDataTableToCsvAsString(DataTable dataTable);
        void UploadCsvDataToFtp(string stream, string filePath);
    }
}