using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Customers
{
    public class CustomerConfiguration : EntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Customer", "dbo");

            builder.Property(customer => customer.Code)
                .HasMaxLength(Customer.MaximumCodeLength);

            builder.Property(customer => customer.CustomerType)
                .IsRequired();

            builder.Property<long?>("PersonId");
            builder.Property<long?>("BusinessId");

            builder
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey("PersonId")  // Shadow property
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne<Business>()
                .WithMany()
                .HasForeignKey("BusinessId")  // Shadow property
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasIndex("PersonId")
                .HasDatabaseName("IX_PersonId");

            builder
                .HasIndex("BusinessId")
                .HasDatabaseName("IX_BusinessId");

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

            builder.Ignore(customer => customer.CustomerEntity);
            builder.Ignore(customer => customer.Address);
            builder.Ignore(customer => customer.Phones);
            builder.Ignore(customer => customer.Emails);
        }
    }
}
