using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CustomerVehicleManagement.Api.Configurations.Payables
{
    public class VendorConfiguration : EntityConfiguration<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            base.Configure(builder);
            builder.ToTable("Vendor", "dbo");

            builder.Property(vendor => vendor.VendorCode)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(vendor => vendor.Name)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(vendor => vendor.Notes)
                .HasMaxLength(10000);
            builder.Property(vendor => vendor.VendorRole)
                .IsRequired();

            // Value Object: DefaultPaymentMethod
            builder.OwnsOne(
                vendor => vendor.DefaultPaymentMethod, builder =>
                {
                    builder.WithOwner();
                    builder.Navigation(paymentMethod => paymentMethod.PaymentMethod)
                        .UsePropertyAccessMode(PropertyAccessMode.Property);
                });
            builder.OwnsOne(vendor => vendor.DefaultPaymentMethod)
                .Property(defaultPaymentMethod => defaultPaymentMethod.AutoCompleteDocuments)
                .IsRequired()
                .HasColumnName("DefaultPaymentMethodAutoCompleteDocuments");

            // Value Object: Address
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.AddressLine)
                   .HasColumnName("AddressLine")
                   .HasMaxLength(255);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(255);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(15);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(2)
                   .HasConversion(
                        state => state.ToString(),
                        state => (State)Enum.Parse(typeof(State), state));
        }
    }
}
