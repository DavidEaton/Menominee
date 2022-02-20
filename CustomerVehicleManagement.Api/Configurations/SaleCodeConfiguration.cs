using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class SaleCodeConfiguration : EntityConfiguration<SaleCode>
    {
        public override void Configure(EntityTypeBuilder<SaleCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("SaleCode", "dbo");

            builder.Ignore(saleCode => saleCode.TrackingState);

            builder.Property(saleCode => saleCode.Code)
                .IsRequired();
            builder.Property(saleCode => saleCode.Name)
                .IsRequired();
        }
    }
}
