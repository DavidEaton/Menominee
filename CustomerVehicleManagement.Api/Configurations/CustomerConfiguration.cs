using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class CustomerConfiguration : EntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Customer", "dbo");
            builder.Ignore(customer => customer.TrackingState);

            // Value Object: ContactPreferences
            builder.OwnsOne(customer => customer.ContactPreferences)
                   .Property(contactPreferences => contactPreferences.AllowEmail)
                   .HasColumnName("AllowEmail")
                   .IsRequired();
            builder.OwnsOne(customer => customer.ContactPreferences)
                   .Property(contactPreferences => contactPreferences.AllowMail)
                   .HasColumnName("AllowMail")
                   .IsRequired();
            builder.OwnsOne(customer => customer.ContactPreferences)
                   .Property(contactPreferences => contactPreferences.AllowSms)
                   .HasColumnName("AllowSms")
                   .IsRequired();
        }
    }
}
