using NCS.DSS.IpsosMori.Ioc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NCS.DSS.IpsosMori.DB;
using NCS.DSS.IpsosMori.DemographicExtractHttpTrigger.Service;
using NCS.DSS.IpsosMori.Helpers;
using NCS.DSS.IpsosMori.SatisfactionExtractHttpTrigger.Service;

[assembly: FunctionsStartup(typeof(FunctionStartupExtension))]

namespace NCS.DSS.IpsosMori.Ioc
{
    public class FunctionStartupExtension : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAzureSqlDbProvider, AzureSqlDbProvider>();
            builder.Services.AddSingleton<ISatisfactionExtractService, SatisfactionExtractService>();
            builder.Services.AddSingleton<IDemographicExtractService, DemographicExtractService>();

            builder.Services.AddSingleton<ICsvHelper, CsvHelper>();
            builder.Services.AddSingleton<IFtpHelper, FtpHelper>();
        }
    }
}
