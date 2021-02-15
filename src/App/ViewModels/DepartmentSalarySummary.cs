using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace App.ViewModels
{
    public class DepartmentSalarySummary
    {
        public string DepartmentName { get; set; } 
        public decimal? AverageSalary { get; set; } 
        public int EmployeesCount { get; set; }         
        public IEnumerable<DepartmentEmployee> Employees { get; set; } = new List<DepartmentEmployee>();        
    }

    public class DepartmentEmployee
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [JsonIgnore]
        public decimal Salary { get; set; }  
    } 
}