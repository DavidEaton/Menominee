using Menominee.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Vehicles
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
