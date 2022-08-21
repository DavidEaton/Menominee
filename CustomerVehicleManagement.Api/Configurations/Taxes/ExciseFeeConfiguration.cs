﻿using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Taxes
{
    public class ExciseFeeConfiguration : EntityConfiguration<ExciseFee>
    {
        public override void Configure(EntityTypeBuilder<ExciseFee> builder)
        {
            base.Configure(builder);
            builder.ToTable("ExciseFee", "dbo");

            builder.Property(fee => fee.Description)
                .IsRequired();
            builder.Property(fee => fee.FeeType)
                .HasDefaultValue(ExciseFeeType.Flat);
            builder.Property(fee => fee.Amount)
                .HasDefaultValue(0);
        }
    }
}
