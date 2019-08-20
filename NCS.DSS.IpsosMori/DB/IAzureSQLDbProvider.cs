using System.Data;

namespace NCS.DSS.IpsosMori.DB
{
    public interface IAzureSqlDbProvider
    {

        DataTable GetDemographicExtract();
        DataTable GetSatisfactionExtract();
    }
}
