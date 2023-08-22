using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class ManufacturerConfiguration : EntityConfiguration<Manufacturer>
    {
        public override void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Manufacturer", "dbo");

            builder.Property(manufacturer => manufacturer.Id)
                .IsRequired();
            builder.Property(manufacturer => manufacturer.Name)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(manufacturer => manufacturer.Prefix)
                .HasMaxLength(4);
        }
    }
}
