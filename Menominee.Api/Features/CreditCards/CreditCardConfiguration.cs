using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.CreditCards
{
    public class CreditCardConfiguration : EntityConfiguration<CreditCard>
    {
        public override void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            base.Configure(builder);
            builder.ToTable("CreditCard", "dbo");

            builder.Property(cc => cc.Name)
                .IsRequired();
        }
    }
}
