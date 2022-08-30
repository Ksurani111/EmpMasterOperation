using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeModel>> GetAllEmployees();

        Task<EmployeeModel> GetEmployeeById(int id);

        Task<int> AddEmp(EmployeeModel emp);

        Task<int> UpdateEmp(EmployeeModel emp);

        Task<int> DeleteEmp(int id);
    }
}
