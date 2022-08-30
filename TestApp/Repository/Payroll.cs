using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Library;
using TestApp.Repository;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace TestApp.Repository
{
    public class Payroll : IPayroll
    {
        private readonly IConnectionString _connectionString;
        private StoreLogs strLogs = new StoreLogs();

        public Payroll(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> StorePayrollData()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    
                    int result = await con.ExecuteScalarAsync<int>("spx_InsertPayrolData", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }

            catch (Exception e)
            {
                strLogs.SaveLogs(e.InnerException.ToString());
                return 0;
            }
        }
    }
}
