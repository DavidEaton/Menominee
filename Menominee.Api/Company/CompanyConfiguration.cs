using Menominee.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Menominee.Domain.Entities;
namespace Menominee.Api.Company
{
    public class CompanyConfiguration : EntityConfiguration<Entities.Company>
    {
        public override void Configure(EntityTypeBuilder<Entities.Company> builder)
        {
            base.Configure(builder);
            builder.ToTable("Company", "dbo");

        }
    }
}
