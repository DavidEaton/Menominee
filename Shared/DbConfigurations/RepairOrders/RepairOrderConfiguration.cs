using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderConfiguration : EntityConfiguration<RepairOrder>
    {
        public override void Configure(EntityTypeBuilder<RepairOrder> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("RepairOrders", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
