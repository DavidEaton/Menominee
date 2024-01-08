using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Menominee.Api.Features.Contactables.Businesses
{
    public class BusinessConfiguration : EntityConfiguration<Business>
    {
        public override void Configure(EntityTypeBuilder<Business> builder)
        {
            base.Configure(builder);
            builder.ToTable("Business", "dbo");

            builder.Property(business => business.Notes)
                .HasMaxLength(10_000);

            // Value Object: BusinessName
            builder.OwnsOne(business => business.Name)
                .Property(name => name.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            // Value Object: Address
            builder.OwnsOne(business => business.Address)
                .Property(address => address.AddressLine1)
                .HasColumnName("AddressLine1")
                .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.AddressLine2)
                .HasColumnName("AddressLine2")
                .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.City)
                .HasColumnName("AddressCity")
                .HasMaxLength(Address.CityMaximumLength);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.PostalCode)
                .HasColumnName("AddressPostalCode")
                .HasMaxLength(Address.PostalCodeMaximumLength);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.State)
                .HasColumnName("AddressState")
                .HasMaxLength(2)
                .HasConversion(
                    stringType => stringType.ToString(),
                    stringType => (State)Enum.Parse(typeof(State), stringType));
        }
    }
}
