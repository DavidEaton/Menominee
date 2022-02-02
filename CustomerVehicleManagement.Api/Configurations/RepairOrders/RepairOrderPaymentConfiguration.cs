using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderPaymentConfiguration : EntityConfiguration<RepairOrderPayment>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderPayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderPayments", "dbo");
            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.PaymentMethod)
               .HasDefaultValue(PaymentMethod.Cash)
               .IsRequired();
        }
    }
}
