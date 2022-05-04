using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.CreditCards
{
    public class CreditCardConfiguration : EntityConfiguration<CreditCard>
    {
        public override void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            base.Configure(builder);
            builder.ToTable("CreditCard", "dbo");

            builder.Ignore(cc => cc.TrackingState);

            builder.Property(cc => cc.Name)
                .IsRequired();
        }
    }
}
