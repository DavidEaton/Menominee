using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class EmailConfiguration : EntityConfiguration<Email>
    {
        public override void Configure(EntityTypeBuilder<Email> builder)
        {
            base.Configure(builder);
            builder.ToTable("Email", "dbo");

            // An email address must not exceed 254 characters.
            // The maximum length specified in RFC 5321
            // (http://tools.ietf.org/html/rfc5321#section-4.5.3) states:
            // The maximum total length of a reverse-path or forward-path is 256 characters.
            builder.Property(email => email.Address)
                .HasMaxLength(254)
                .IsRequired();
            builder.Property(email => email.IsPrimary)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
