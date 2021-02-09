using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using App.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public partial class ApplicationDbContext : DbContext
    {
        protected void OnHasData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "FINANCE" },
                new Department { Id = 2, Name = "MARKETING" },
                new Department { Id = 3, Name = "PRODUCTION" },
                new Department { Id = 4, Name = "COVID" },
                new Department { Id = 5, Name = "Rebranding" }
            );
        }

        public static void Seed(ApplicationDbContext context, IServiceProvider sp)
        {
            if(context.Departments.Any() && context.Employees.Any() == false)
            {                
                var env = sp.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
                var sepdec = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                
                var employees = File.ReadAllLines(Path.Combine(env.ContentRootPath, "Files", "employees.csv"))
                    .Skip(1) // header
                    .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                    .Select(col => new Employee 
                    { 
                        Id = int.TryParse(col[0], out var Id) ? Id : default,
                        FirstName = col[1],
                        LastName = col[2],
                        Salary = decimal.TryParse(col[3].Replace(",", sepdec).Replace(".", sepdec), out var Salary) ? Salary : default,
                        DepartmentId = int.TryParse(col[4], out var DepartmentId) ? DepartmentId : default
                    });
                if(employees.Any())
                {
                    context.Database.CreateExecutionStrategy().Execute(() =>
                    {
                        using (var tx = context.Database.BeginTransaction())
                        {                            
                            context.Employees.AddRange(employees);
                            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Employees ON;");
                            context.SaveChanges();
                            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Employees OFF");
                            tx.Commit();
                        }
                    });
                }
            }
        }
    }
}
