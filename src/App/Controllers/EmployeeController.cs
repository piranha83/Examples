using System.Linq;
using System.Threading.Tasks;
using App.Extensions;
using App.Filters;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Maps;

namespace App.Controllers
{
    [ApiController]
    [ValidateModel]
    [Route("api/[controller]")]
    public class EmployeeController
    {
        readonly IRepository<Employee> _employeeRepository;
        readonly IRepository<Department> _departmentRepository;
        readonly ISession _session;

        public EmployeeController(IRepository<Employee> employeeRepository,
        IRepository<Department> departmentRepository,
        ISession session)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _session = session;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _employeeRepository.Find();            
            return new JsonResult(data);
        }        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _employeeRepository.Find(id);
            return new JsonResult(data);
        }
        
        [HttpGet]
        [Route("Department/AvgSalarySummary")]
        public async Task<IActionResult> DepartmentAvgSalary()
        {
            /*var group = await(
                from d in _departmentRepository.AsQueryable()
                join e in _employeeRepository.AsQueryable().DefaultIfEmpty()
                on d.Id equals e.DepartmentId into g
                from e in g.DefaultIfEmpty()
                select new { Salary = (decimal?)e.Salary, EmployeeName = e.FullName, DepartmentName = d.Name }                
            ).ToListAsync();

            var data = group.GroupBy(m=>m.DepartmentName).Select(m=> new DepartmentSalarySummary 
            {          
                DepartmentName = m.Key,
                AverageSalary = Math.Round(m.Average(m=>m.Salary).GetValueOrDefault(), 2),
                EmployeesCount = m.Count(m=>m.Salary.HasValue),
                Employees = m.Select(m=>m.EmployeeName)
            }); OR */ 
            var data = await _departmentRepository.AsQueryable()
                .Include(m=>m.Employees)
                .Select(m=>Map.MapDepartmentSalarySummary(m))
                .ToListAsync();            

            return new JsonResult(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee data)
        {
            _employeeRepository.Add(data);
            await _session.Save();
            return new JsonResult(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Employee data)
        {
            data.Id = id;
            _employeeRepository.Update(data);
            await _session.Save();
            return new JsonResult(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _employeeRepository.Delete(id);
            await _session.Save();
            return new JsonResult(true);
        }
    }
}
