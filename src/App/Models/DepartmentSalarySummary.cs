using System.Collections.Generic;

namespace App.Models
{
    public class DepartmentSalarySummary
    {
        public string DepartmentName { get; set; } 
        public decimal? AverageSalary { get; set; } 
        public int EmployeesCount { get; set; }         
        public IEnumerable<string> Employees { get; set; } = new List<string>();
    }
}