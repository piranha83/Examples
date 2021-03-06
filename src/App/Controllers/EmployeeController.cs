using System.Linq;
using System.Threading.Tasks;
using App.Extensions;
using App.Filters;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [ValidateModel]
    [Route("api/[controller]")]
    public class EmployeeController
    {
        readonly IRepository<Employee> _employeeRepository;
        readonly IRepository<Department> _departmentRepository;
        readonly ISession _session;
        readonly IDistributedCache _cache;

        public EmployeeController(
            IRepository<Employee> employeeRepository,
            IRepository<Department> departmentRepository,
            ISession session,
            IDistributedCache cache)
        {           
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _session = session;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var data = await _employeeRepository.Find();  
            if(data == null) 
                new NotFoundResult();          
            return new JsonResult(data);
        }        

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _employeeRepository.Find(id);
            if(data == null)
                new NotFoundResult();
            return new JsonResult(data);
        }
        
        [AllowAnonymous]
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
            var data = await _cache.GetOrCreateAsync(nameof(DepartmentAvgSalary), () => 
            {
                return _departmentRepository.AsQueryable()
                    .Include(m=>m.Employees)
                    .Select(m=>Map.MapDepartmentSalarySummary(m))
                    .ToListAsync();
            }, TimeSpan.FromHours(8));            
            return new JsonResult(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee data)
        {
            _employeeRepository.Add(data);
            await _session.Save();
            await _cache.RemoveAsync(nameof(DepartmentAvgSalary));
            return new JsonResult(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Employee data)
        {
            data.Id = id;
            _employeeRepository.Update(data);
            await _session.Save();
            await _cache.RemoveAsync(nameof(DepartmentAvgSalary));
            return new JsonResult(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _employeeRepository.Delete(id);
            await _session.Save();
            await _cache.RemoveAsync(nameof(DepartmentAvgSalary));
            return new JsonResult(true);
        }
    }
}
