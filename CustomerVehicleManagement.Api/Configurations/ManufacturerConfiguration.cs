using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class ManufacturerConfiguration : EntityConfiguration<Manufacturer>
    {
        public override void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Manufacturer", "dbo");

            builder.Ignore(mfr => mfr.TrackingState);

            builder.Property(mfr => mfr.Code)
                .IsRequired();
            builder.Property(mfr => mfr.Name)
                .IsRequired();
        }
    }
}
