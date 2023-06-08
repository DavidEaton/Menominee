﻿using CustomerVehicleManagement.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Vehicles
{
    public class VehicleConfiguration : EntityConfiguration<Domain.Entities.Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Vehicle> builder)
        {
            base.Configure(builder);
            builder.ToTable("Vehicle", "dbo");
        }
    }
}