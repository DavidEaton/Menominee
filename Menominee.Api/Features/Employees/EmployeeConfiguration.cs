using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Employees
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

            builder.Property(employee => employee.SSN)
                .HasMaxLength(Employee.MaximumSSNLength)
                .IsRequired(false);

            builder.Property(employee => employee.CertificationNumber)
                .HasMaxLength(Employee.MaximumCertificationNumberLength)
                .IsRequired(false);

            builder.Property(employee => employee.Active)
                .HasDefaultValue(true);

            builder.Property(employee => employee.PrintedName)
                .HasMaxLength(Employee.MaximumPrintedNameLength)
                .IsRequired(false);

            builder.Property(employee => employee.ExpenseCategory)
                .HasDefaultValue(EmployeeExpenseCategory.CostOfDirectLabor);

            builder.Property(employee => employee.BenefitLoad)
                .HasDefaultValue(0.0);
        }
    }
}
