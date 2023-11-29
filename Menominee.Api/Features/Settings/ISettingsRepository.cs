using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Settings
{
    public interface ISettingsRepository
    {
        // TODO MEN-1022: Align pattern with other repositories in our solution.
        void Add(ConfigurationSetting entity);
        Task<IReadOnlyList<SettingToRead>> GetListAsync();
        Task<SettingToRead> GetAsync(SettingName settingName);
        Task<IReadOnlyList<SettingToRead>> SaveListAsync(List<SettingToWrite> settings);
        Task<SettingToRead> SaveAsync(SettingToWrite setting);
        Task<IReadOnlyList<SettingToRead>> UpdateListAsync(List<SettingToWrite> setting);
        Task<SettingToRead> UpdateAsync(SettingToWrite setting);
        Task<IReadOnlyList<SettingToRead>> GetByGroupAsync(SettingGroup group);
        Task<ConfigurationSetting> GetEntityAsync(long id);
        Task SaveChangesAsync();
    }
}
