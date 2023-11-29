using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Menominee.Api.Features.Payables.Vendors
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
                   .Property(address => address.AddressLine1)
                   .HasColumnName("AddressLine1")
                   .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.AddressLine2)
                   .HasColumnName("AddressLine2")
                   .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(Address.CityMaximumLength);
            builder.OwnsOne(vendor => vendor.Address)
                   .Property(address => address.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(Address.PostalCodeMaximumLength);
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
