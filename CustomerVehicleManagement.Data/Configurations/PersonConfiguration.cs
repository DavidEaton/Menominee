using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Person", "dbo");
            builder.Ignore(p => p.TrackingState);

            // Converting enum values to and from strings in the database
            var converter = new EnumToStringConverter<Gender>();
            builder.Property(p => p.Gender)
                .HasConversion(converter);

            // Value Object: Name
            builder.OwnsOne(p => p.Name)
                   .Property(p => p.FirstName)
                   .HasColumnName("FirstName")
                   .IsRequired().HasMaxLength(255);
            builder.OwnsOne(p => p.Name)
                   .Property(p => p.LastName)
                   .HasColumnName("LastName")
                   .IsRequired().HasMaxLength(255);
            builder.OwnsOne(p => p.Name)
                   .Property(p => p.MiddleName)
                   .HasColumnName("MiddleName")
                   .HasMaxLength(255);

            // Value Object: Address
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.AddressLine)
                   .HasColumnName("AddressLine")
                   .HasMaxLength(255);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(255);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.CountryCode)
                   .HasColumnName("AddressCountryCode")
                   .HasMaxLength(2);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(15);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(255);

            // Value Object: DriversLicence
            builder.OwnsOne(p => p.DriversLicence)
                .Property(p => p.Number)
                .HasColumnName("DriversLicenseNumber")
                .HasMaxLength(50);
            builder.OwnsOne(p => p.DriversLicence)
                .Property(p => p.State)
                .HasColumnName("DriversLicenseState")
                .HasMaxLength(2);
            builder.OwnsOne(p => p.DriversLicence)
                .OwnsOne(p => p.ValidRange)
                .Property(p => p.Start)
                .HasColumnName("DriversLicenseIssued");
            builder.OwnsOne(p => p.DriversLicence)
                .OwnsOne(p => p.ValidRange)
                .Property(p => p.End)
                .HasColumnName("DriversLicenseExpiry");
        }
    }
}
