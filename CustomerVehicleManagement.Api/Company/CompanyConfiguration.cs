using CustomerVehicleManagement.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = CustomerVehicleManagement.Domain.Entities;
namespace CustomerVehicleManagement.Api.Company
{
    public class CompanyConfiguration : EntityConfiguration<Entities.Company>
    {
        public override void Configure(EntityTypeBuilder<Entities.Company> builder)
        {
            base.Configure(builder);
            builder.ToTable("Company", "dbo");

        }
    }
}
