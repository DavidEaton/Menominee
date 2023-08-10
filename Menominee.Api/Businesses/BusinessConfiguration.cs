using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Menominee.Common.Enums;
using System;
using Menominee.Api.Configurations;

namespace Menominee.Api.Businesses
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
                .Property(address => address.AddressLine)
                .HasColumnName("AddressLine")
                .HasMaxLength(255);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.City)
                .HasColumnName("AddressCity")
                .HasMaxLength(255);
            builder.OwnsOne(business => business.Address)
                .Property(address => address.PostalCode)
                .HasColumnName("AddressPostalCode")
                .HasMaxLength(15);
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
