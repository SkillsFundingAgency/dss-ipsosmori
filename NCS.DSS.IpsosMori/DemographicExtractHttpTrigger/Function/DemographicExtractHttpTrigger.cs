using System;
using System.Globalization;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NCS.DSS.IpsosMori.DemographicExtractHttpTrigger.Service;

namespace NCS.DSS.IpsosMori.DemographicExtractHttpTrigger.Function
{
    public class DemographicExtractHttpTrigger
    {
        public const string Schedule = "%PollingSchedule%";
        private readonly IDemographicExtractService _demographicExtractService;

        public DemographicExtractHttpTrigger(IDemographicExtractService demographicExtractService)
        {
            _demographicExtractService = demographicExtractService;
        }

        [FunctionName("DemographicExtractHttpTrigger")]       
        public void Run([TimerTrigger(Schedule, RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

            var fileName = string.Format("{0}_{1}_{2}.{3}", "Demographic",
                DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture), DateTime.UtcNow.Year, "csv");

            log.LogInformation(string.Format("attempting to get data for: {0}", fileName));

            var demographicDataTable = _demographicExtractService.GetDemographicExtractDataTable();

            if (demographicDataTable == null || demographicDataTable.Rows.Count == 0)
            {
                log.LogError("no data returned from Stored Procedure.");
                return;
            }

            log.LogInformation("attempting to convert data to csv format");

            var csvData = _demographicExtractService.ConvertDataTableToCsvAsString(demographicDataTable);

            if (csvData == null)
            {
                log.LogError("Unable to convert Data Table to CSV format.");
                return;
            }

            log.LogInformation("attempting to upload csv data to FTP");

            _demographicExtractService.UploadCsvDataToFtp(csvData, fileName);

            log.LogInformation("successfully upload csv data to FTP");
        }

    }
}
