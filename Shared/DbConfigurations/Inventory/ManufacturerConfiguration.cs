using MenomineePlayWASM.Shared.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.DbConfigurations.Inventory
{
    public class ManufacturerConfiguration : EntityConfiguration<Manufacturer>
    {
        public override void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Manufacturers", "dbo");

            builder.Ignore(mfr => mfr.TrackingState);

            builder.Property(mfr => mfr.MfrId)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(mfr => mfr.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(mfr => mfr.Prefix)
                   .HasMaxLength(4);
        }
    }
}
