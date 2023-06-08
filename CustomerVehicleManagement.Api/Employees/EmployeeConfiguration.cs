using CustomerVehicleManagement.Api.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Employees
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
