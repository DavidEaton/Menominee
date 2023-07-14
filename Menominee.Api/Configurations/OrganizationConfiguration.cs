﻿using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Menominee.Common.Enums;
using System;

namespace Menominee.Api.Configurations
{
    public class OrganizationConfiguration : EntityConfiguration<Organization>
    {
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            base.Configure(builder);
            builder.ToTable("Organization", "dbo");

            builder.Property(organization => organization.Notes)
                .HasMaxLength(10_000);

            // Value Object: OrganizationName
            builder.OwnsOne(organization => organization.Name)
                .Property(name => name.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            // Value Object: Address
            builder.OwnsOne(organization => organization.Address)
                .Property(address => address.AddressLine)
                .HasColumnName("AddressLine")
                .HasMaxLength(255);
            builder.OwnsOne(organization => organization.Address)
                .Property(address => address.City)
                .HasColumnName("AddressCity")
                .HasMaxLength(255);
            builder.OwnsOne(organization => organization.Address)
                .Property(address => address.PostalCode)
                .HasColumnName("AddressPostalCode")
                .HasMaxLength(15);
            builder.OwnsOne(organization => organization.Address)
                .Property(address => address.State)
                .HasColumnName("AddressState")
                .HasMaxLength(2)
                .HasConversion(
                    stringType => stringType.ToString(),
                    stringType => (State)Enum.Parse(typeof(State), stringType));
        }
    }
}