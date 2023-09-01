using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Menominee.Api.Configurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);
            builder.ToTable("Person", "dbo");

            // Value Object: Name
            builder.OwnsOne(person => person.Name)
                   .Property(personName => personName.FirstName)
                   .HasColumnName("FirstName")
                   .IsRequired()
                   .HasMaxLength(255);
            builder.OwnsOne(person => person.Name)
                   .Property(personName => personName.LastName)
                   .HasColumnName("LastName")
                   .IsRequired().HasMaxLength(255);
            builder.OwnsOne(person => person.Name)
                   .Property(personName => personName.MiddleName)
                   .HasColumnName("MiddleName")
                   .HasMaxLength(255);

            // Value Object: Address
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.AddressLine1)
                   .HasColumnName("AddressLine1")
                   .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.AddressLine2)
                   .HasColumnName("AddressLine2")
                   .HasMaxLength(Address.AddressMaximumLength);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(Address.CityMaximumLength);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(Address.PostalCodeMaximumLength);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(2)
                   .HasConversion(
                        state => state.ToString(),
                        state => (State)Enum.Parse(typeof(State), state));

            // Value Object: DriversLicense
            builder.OwnsOne(person => person.DriversLicense)
                .Property(driversLicense => driversLicense.Number)
                .HasColumnName("DriversLicenseNumber")
                .HasMaxLength(50);
            builder.OwnsOne(person => person.DriversLicense)
                .Property(driversLicense => driversLicense.State)
                .HasColumnName("DriversLicenseState")
                   .HasMaxLength(2)
                   .HasConversion(
                        state => state.ToString(),
                        state => (State)Enum.Parse(typeof(State), state));
            builder.OwnsOne(person => person.DriversLicense)
                .OwnsOne(driversLicense => driversLicense.ValidDateRange)
                .Property(dateTimeRange => dateTimeRange.Start)
                .HasColumnName("DriversLicenseIssued");
            builder.OwnsOne(person => person.DriversLicense)
                .OwnsOne(driversLicense => driversLicense.ValidDateRange)
                .Property(dateTimeRange => dateTimeRange.End)
                .HasColumnName("DriversLicenseExpiry");
        }
    }
}
