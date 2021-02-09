using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Configuration
{
    public class DepartmentConfiuration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(m=>m.Employees).WithOne(m=>m.Department).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(32);             
        }
    }
}