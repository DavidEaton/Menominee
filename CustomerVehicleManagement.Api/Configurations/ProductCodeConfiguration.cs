using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class ProductCodeConfiguration : EntityConfiguration<ProductCode>
    {
        public override void Configure(EntityTypeBuilder<ProductCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProductCode", "dbo");

            builder.Ignore(pc => pc.TrackingState);

            builder.Property(pc => pc.Manufacturer)
                .IsRequired();
            builder.Property(pc => pc.Code)
                .IsRequired();
            builder.Property(pc => pc.Name)
                .IsRequired();
        }
    }
}
