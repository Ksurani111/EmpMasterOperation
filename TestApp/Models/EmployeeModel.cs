using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public class EmployeeModel
    {
    
        public int? ID { get; set; }
        [Required(ErrorMessage = "*")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "*")]
        public string? City { get; set; }
        [Required(ErrorMessage = "*")]
        public int Salary { get; set; }
        [Required(ErrorMessage = "*")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime DOJ { get; set; }
    }
}
