using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Configuration
{
    public class EmployeeConfiuration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(m=>m.Department).WithMany(m=>m.Employees).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(32); 
            builder.Property(e => e.LastName).HasMaxLength(32); 
            builder.Property(e => e.Salary).HasPrecision(10,2); 
            builder.Ignore(e => e.FullName);            
        }
    }
}