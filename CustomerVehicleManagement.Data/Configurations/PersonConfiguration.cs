using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Person", "dbo");
            builder.Ignore(person => person.TrackingState);

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
                   .Property(address => address.AddressLine)
                   .HasColumnName("AddressLine")
                   .HasMaxLength(255);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(255);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(15);
            builder.OwnsOne(person => person.Address)
                   .Property(address => address.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(255);

            // Value Object: DriversLicense
            builder.OwnsOne(person => person.DriversLicense)
                .Property(driversLicense => driversLicense.Number)
                .HasColumnName("DriversLicenseNumber")
                .HasMaxLength(50);
            builder.OwnsOne(person => person.DriversLicense)
                .Property(driversLicense => driversLicense.State)
                .HasColumnName("DriversLicenseState")
                .HasMaxLength(2);
            builder.OwnsOne(person => person.DriversLicense)
                .OwnsOne(driversLicense => driversLicense.ValidRange)
                .Property(dateTimeRange => dateTimeRange.Start)
                .HasColumnName("DriversLicenseIssued");
            builder.OwnsOne(person => person.DriversLicense)
                .OwnsOne(driversLicense => driversLicense.ValidRange)
                .Property(dateTimeRange => dateTimeRange.End)
                .HasColumnName("DriversLicenseExpiry");
        }
    }
}
