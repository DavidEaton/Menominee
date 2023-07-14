﻿using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.RepairOrders
{
    public class RepairOrderServiceConfiguration : EntityConfiguration<RepairOrderService>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderService> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderService", "dbo");
        }
    }
}