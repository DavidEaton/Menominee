using Menominee.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Settings
{
    public class SettingConfiguration : EntityConfiguration<ConfigurationSetting>
    {
        public override void Configure(EntityTypeBuilder<ConfigurationSetting> builder)
        {
            base.Configure(builder);
            builder.ToTable("Setting", "dbo");

            builder.Property(setting => setting.SettingName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(setting => setting.SettingGroup)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(setting => setting.Value)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(setting => setting.SettingValueType)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
