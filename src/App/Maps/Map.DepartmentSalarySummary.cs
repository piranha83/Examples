using System;
using System.Linq;
using App.Models;
using App.ViewModels;

namespace App.Maps
{
    public partial class Map
    {
        public static DepartmentSalarySummary MapDepartmentSalarySummary(Department m)
        {
            var employees = m.Employees.Select(MapDepartmentEmployee).ToList();
            return new DepartmentSalarySummary {
                DepartmentName = m.Name,
                AverageSalary = employees.Count > 0 
                    ? Math.Round(employees.Average(m=>m.Salary), 2) 
                    : 0M,
                EmployeesCount = employees.Count,
                Employees = employees
            };
        }
        public static DepartmentEmployee MapDepartmentEmployee(Employee m)
            => new DepartmentEmployee { Id = m.Id, FullName = m.FullName, Salary = m.Salary };
    }
}