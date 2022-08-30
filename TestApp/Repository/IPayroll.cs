using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Repository
{
    public interface IPayroll
    {
        Task<int> StorePayrollData();
    }
}
