using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Repository;

using TestApp.Models;

namespace TestApp.Controllers
{
    public class DashBoardController : Controller
    {
        private IEmployeeRepository _employeerepository;

        public DashBoardController(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }
        public async Task<IActionResult> Index()
        {

            var empData = await _employeerepository.GetAllEmployees();
            ViewData["EmpCount"] = empData.Count();
            return View();
        }
    }
}