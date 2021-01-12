using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class CustomerConfiguration : EntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Customer", "dbo");
            builder.Ignore(p => p.Entity);
            builder.Ignore(p => p.TrackingState);
        }
    }
}
