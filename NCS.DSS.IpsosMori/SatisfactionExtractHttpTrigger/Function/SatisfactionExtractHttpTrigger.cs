using System;
using System.Globalization;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NCS.DSS.IpsosMori.SatisfactionExtractHttpTrigger.Service;

namespace NCS.DSS.IpsosMori.SatisfactionExtractHttpTrigger.Function
{
    public class SatisfactionExtractHttpTrigger
    {
        public const string Schedule = "%PollingSchedule%";
        private readonly ISatisfactionExtractService _satisfactionExtractService;

        public SatisfactionExtractHttpTrigger(ISatisfactionExtractService satisfactionExtractService)
        {
            _satisfactionExtractService = satisfactionExtractService;
        }

        [FunctionName("SatisfactionExtractHttpTrigger")]
        public void Run([TimerTrigger(Schedule)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

            var fileName = string.Format("{0}_{1}_{2}.{3}", "Satisfaction",
                DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture), DateTime.UtcNow.Year, "csv");

            log.LogInformation(string.Format("attempting to get data for: {0}", fileName));

            var satisfactionDataTable = _satisfactionExtractService.GetSatisfactionExtractDataTable();

            if (satisfactionDataTable == null || satisfactionDataTable.Rows.Count == 0)
            {
                log.LogError("no data returned from Stored Procedure.");
                return;
            }

            log.LogInformation("attempting to convert data to csv format");

            var csvData = _satisfactionExtractService.ConvertDataTableToCsvAsString(satisfactionDataTable);

            if (csvData == null)
            {
                log.LogError("Unable to convert Data Table to CSV format.");
                return;
            }

            log.LogInformation("attempting to upload csv data to FTP");

            _satisfactionExtractService.UploadCsvDataToFtp(csvData, fileName);

            log.LogInformation("successfully upload csv data to FTP");

        }

    }
}
