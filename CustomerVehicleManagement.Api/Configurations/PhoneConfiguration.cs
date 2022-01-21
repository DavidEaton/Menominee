using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class PhoneConfiguration : EntityConfiguration<Phone>
    {
        public override void Configure(EntityTypeBuilder<Phone> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Phone", "dbo");

            builder.Ignore(phone => phone.TrackingState);

            builder.Property(phone => phone.Number)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(phone => phone.PhoneType)
                .IsRequired();
            builder.Property(phone => phone.IsPrimary)
                .IsRequired();
        }
    }
}
