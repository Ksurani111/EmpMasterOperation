using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Repository;
 
using TestApp.Models;
using Hangfire;
using TestApp.Library;
using Hangfire.Storage;
using Hangfire.Common;

 

namespace TestApp.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeerepository;
        private IPayroll _emppayroll;

        
        public EmployeeController(IEmployeeRepository employeerepository, IPayroll emppayroll)
        {
            _employeerepository = employeerepository;
            _emppayroll = emppayroll;
          

        }
        public const string tagRecurringJob = "recurring-job";
        public const string tagStopJob = "recurring-jobs-stop";

        

        public async Task<IActionResult> Index()
        {
            var empData = await _employeerepository.GetAllEmployees();

           

         //   RecurringJob.AddOrUpdate(() => _emppayroll.StorePayrollData(), "* * * * *");
     

            //IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();

            //BackgroundJob.Enqueue(() => str());

            //var jobData = monitor.EnqueuedJobs("default", 0,1);

            return View(empData);
        }

        [Hangfire.AutomaticRetry(Attempts = 5)]
        public string str()
        {
            IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();
            var jobData = monitor.EnqueuedJobs("default", 0, 1);
          //  throw new Exception();
             return "krs";
        }

        public async Task<IActionResult> Stop()
        {
            var empData = await _employeerepository.GetAllEmployees();
            using (var connection = JobStorage.Current.GetConnection())

              

            using (var transaction = connection.CreateWriteTransaction())
            {


                foreach (var recurringJob in connection.GetRecurringJobs())
                {

                    transaction.RemoveFromSet($"{tagRecurringJob}s", recurringJob.Id);
                    transaction.AddToSet($"{tagStopJob}", recurringJob.Id);
                    transaction.Commit();

                }
            }
            return View("_List", empData);
        }
        public async Task<IActionResult> Start()
        {
            var empData = await _employeerepository.GetAllEmployees();
            var data = GetAllJobStopped();

            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction())
            {
                foreach (var recurringJob in data)
                {
                    transaction.RemoveFromSet(tagStopJob, recurringJob.Id);
                    transaction.AddToSet($"{tagRecurringJob}s", recurringJob.Id);
                    transaction.Commit();

                }
            }

            return View("_List", empData);
        }

        public static List<PeriodicJob> GetAllJobStopped()
        {
            var outPut = new List<PeriodicJob>();
            using (var connection = JobStorage.Current.GetConnection())
            {
                var allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    var dto = new PeriodicJob();

                    var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");
                    dto.Id = jobId;
                    dto.TimeZoneId = "UTC"; // Default

                    try
                    {
                        if (dataJob.TryGetValue("Job", out var payload) && !String.IsNullOrWhiteSpace(payload))
                        {
                            var invocationData = InvocationData.DeserializePayload(payload);
                            var job = invocationData.DeserializeJob();
                            dto.Method = job.Method.Name;
                            dto.Class = job.Type.Name;
                        }
                    }
                    catch (JobLoadException ex)
                    {
                        dto.Error = ex.Message;
                    }

                    if (dataJob.ContainsKey("TimeZoneId"))
                    {
                        dto.TimeZoneId = dataJob["TimeZoneId"];
                    }

                    if (dataJob.ContainsKey("NextExecution"))
                    {
                        var tempNextExecution = JobHelper.DeserializeNullableDateTime(dataJob["NextExecution"]);
                    }

                    if (dataJob.ContainsKey("LastJobId") && !string.IsNullOrWhiteSpace(dataJob["LastJobId"]))
                    {
                        dto.LastJobId = dataJob["LastJobId"];

                        var stateData = connection.GetStateData(dto.LastJobId);
                        if (stateData != null)
                        {
                            dto.LastJobState = stateData.Name;
                        }
                    }

                    if (dataJob.ContainsKey("Queue"))
                    {
                        dto.Queue = dataJob["Queue"];
                    }

                    if (dataJob.ContainsKey("LastExecution"))
                    {
                        var tempLastExecution = JobHelper.DeserializeNullableDateTime(dataJob["LastExecution"]);
                    }

                    if (dataJob.ContainsKey("CreatedAt"))
                    {
                        dto.CreatedAt = JobHelper.DeserializeNullableDateTime(dataJob["CreatedAt"]);
                    }

                    if (dataJob.TryGetValue("Error", out var error) && !String.IsNullOrEmpty(error))
                    {
                        dto.Error = error;
                    }

                    dto.Removed = false;
                    dto.JobState = "Stopped";
                    outPut.Add(dto);

                });
            }
            return outPut;
        }

        //DeleteEmployee
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeerepository.DeleteEmp(id);
            if (result == 0)
            {
                var empData = await _employeerepository.GetAllEmployees();
                ViewData["Message"] = "Data Deleted for " + id;
                return View("_List", empData);
            }
            else
            {
                var empData = await _employeerepository.GetAllEmployees();
                ViewData["Message"] = "Something went wrong";
                return View("_List", empData);
            }
        }

        //SaveEmployee
        [HttpPost]
        public async Task<IActionResult> SaveEmployee(EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddUpdate", employee);
            }
            else
            {
                if (employee.ID == null)
                {
                    var result = await _employeerepository.AddEmp(employee);
                    TempData["EmpId"] = 0;
                    if (result == 0)
                    {
                        var empData = await _employeerepository.GetAllEmployees();
                        ViewData["Message"] = "Data Inserted";
                        return View("_List", empData);
                    }
                    else
                    {
                        return View("_AddUpdate");
                    }
                }
                else
                {
                    var result = await _employeerepository.UpdateEmp(employee);
                    if (result == 0)
                    {
                        var empData = await _employeerepository.GetAllEmployees();
                        ViewData["Message"] = "Data Update for Employee Id: " + employee.ID;
                        return View("_List", empData);
                    }
                    else
                    {
                     return View("_List");
                    }
                }
            }
        }

        //AddEmployee
        public IActionResult AddEmployee(int? id)
        {
            if (id != null)
            {
                var emp = _employeerepository.GetEmployeeById(id.GetValueOrDefault());
                EmployeeModel employee = emp.Result;
                TempData["EmpId"] = employee.ID;
                return View("_AddUpdate", employee);
            }
            ViewData["Message"] = "";
            return View("_AddUpdate");
        }
    }
}