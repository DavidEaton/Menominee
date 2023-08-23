using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class CustomerConfiguration : EntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Customer", "dbo");

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

            builder.Property(customer => customer.Code)
                .HasMaxLength(Customer.MaximumCodeLength);

            builder.Ignore(customer => customer.Emails);
            builder.Ignore(customer => customer.Phones);
        }
    }
}
