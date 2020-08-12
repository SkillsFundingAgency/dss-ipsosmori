using System.Data;

namespace NCS.DSS.IpsosMori.Helpers
{
    public interface ICsvHelper
    {
        string ConvertDataTableToCsvAsString(DataTable dataTable);
    }
}