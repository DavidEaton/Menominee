using Menominee.Api.Configurations;
using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Employees
{
    public class EmployeeConfiguration : EntityConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            base.Configure(builder);
            builder.ToTable("Employee", "dbo");

            builder.Property(employee => employee.CompanyEmployeeId)
                .HasMaxLength(4);

            builder.Property(employee => employee.Notes)
                .HasMaxLength(10_000);

        }
    }
}
