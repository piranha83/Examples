using System;
using System.Reflection;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Department> Departments { get; set; }  
        public DbSet<Employee> Employees { get; set; } 
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());            

            OnHasData(modelBuilder);
        } 
    }
}
