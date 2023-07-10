using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class PhoneConfiguration : EntityConfiguration<Phone>
    {
        public override void Configure(EntityTypeBuilder<Phone> builder)
        {
            base.Configure(builder);
            builder.ToTable("Phone", "dbo");

            builder.Property(phone => phone.Number)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(phone => phone.PhoneType)
                .IsRequired();
            builder.Property(phone => phone.IsPrimary)
                .IsRequired();
        }
    }
}
