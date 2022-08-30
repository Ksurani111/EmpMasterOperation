using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using TestApp.Library;

namespace TestApp.Repository
{
    /// <summary>
    ///  Employee Master operations-> Insert, Update, List and Soft Delete
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly IConnectionString _connectionString;
        private StoreLogs strLogs = new StoreLogs();
        public EmployeeRepository(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddEmp(EmployeeModel emp)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    var param = new DynamicParameters();
                    param.Add("@FirstName", emp.FirstName);
                    param.Add("@LastName", emp.LastName);
                    param.Add("@City", emp.City);
                    param.Add("@Salary", emp.Salary);
                    param.Add("@Department", emp.Department);
                    param.Add("@DOJ", emp.DOJ);
                    int result = await con.ExecuteScalarAsync<int>("spx_InsertEmpData", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            
            catch (Exception e)
            {
                strLogs.SaveLogs(e.InnerException.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteEmp(int id)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                   
                    int result = await con.ExecuteScalarAsync<int>("spx_DeleteEmpData", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }

            catch (Exception e)
            {
                strLogs.SaveLogs(e.InnerException.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployees()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", dbType: null);
                    return await con.QueryAsync<EmployeeModel>("spx_ListEmpData", param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception e)
            {
               
                strLogs.SaveLogs(e.InnerException.ToString());
                return null;
            }
        }

        public async Task<EmployeeModel> GetEmployeeById(int id)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                    var EmpById = await con.QueryFirstOrDefaultAsync<EmployeeModel>("spx_ListEmpData", param, commandType: CommandType.StoredProcedure);
                    return EmpById;
                }
            }
            catch (Exception e)
            {

                strLogs.SaveLogs(e.InnerException.ToString());
                return null;
            }
        }

        public async Task<int> UpdateEmp(EmployeeModel emp)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    var param = new DynamicParameters();
                    param.Add("@ID", emp.ID);
                    param.Add("@FirstName", emp.FirstName);
                    param.Add("@LastName", emp.LastName);
                    param.Add("@City", emp.City);
                    param.Add("@Salary", emp.Salary);
                    param.Add("@Department", emp.Department);
                    param.Add("@DOJ", emp.DOJ);
                    int result = await con.ExecuteScalarAsync<int>("spx_UpdateEmpData", param, commandType: CommandType.StoredProcedure);
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
