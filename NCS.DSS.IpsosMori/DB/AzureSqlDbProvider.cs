using System;
using System.Data.SqlClient;
using System.Data;

namespace NCS.DSS.IpsosMori.DB
{
    public class AzureSqlDbProvider : IAzureSqlDbProvider
    {
        private readonly string _sqlConnString = Environment.GetEnvironmentVariable("AzureSQLConnectionString");

        public DataTable GetDemographicExtract()
        {
            return ExecuteStoredProcedure("usp_GetDemographicDataForIpsosMoriIntegration");
        }

        public DataTable GetSatisfactionExtract()
        {
            return ExecuteStoredProcedure("usp_GetSatisfactionDataForIpsosMoriIntegration");
        }

        private DataTable ExecuteStoredProcedure(string storedProcedureName)
        {
            using (var conn = new SqlConnection(_sqlConnString))
            {
                using (var cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            var dt = new DataTable();
                            dt.Load(rdr);

                            return dt;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

    }
}