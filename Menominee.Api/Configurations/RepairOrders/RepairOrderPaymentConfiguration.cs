﻿using Menominee.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.RepairOrders
{
    public class RepairOrderPaymentConfiguration : EntityConfiguration<RepairOrderPayment>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderPayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderPayment", "dbo");

            builder.Property(item => item.PaymentMethod)
               .HasDefaultValue(PaymentMethod.Cash)
               .IsRequired();
        }
    }
}