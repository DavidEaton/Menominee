using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class PhoneConfiguration : EntityConfiguration<Phone>
    {
        public override void Configure(EntityTypeBuilder<Phone> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Phone", "dbo");
            builder.Ignore(p => p.TrackingState);

            builder.Property(p => p.Number)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.PhoneType)
                .HasMaxLength(50)
                .IsRequired();

            // Convert enum values to and from strings in the database
            var converter = new EnumToStringConverter<PhoneType>();
            builder.Property(p => p.PhoneType)
                .HasConversion(converter);

        }
    }
}
