using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Settings
{
    public interface ISettingsRepository
    {
        Task<IReadOnlyList<SettingToRead>> GetSettingsListAsync();
        Task<SettingToRead> GetSetting(SettingName settingName);
        Task<IReadOnlyList<SettingToRead>> SaveSettingsListAsync(List<SettingToWrite> settings);
        Task<SettingToRead> SaveSetting(SettingToWrite settingToWrite);
        Task<IReadOnlyList<SettingToRead>> UpdateSettingsListAsync(List<SettingToWrite> settings);
        Task<SettingToRead> UpdateSetting(SettingToWrite setting);
        Task<IReadOnlyList<SettingToRead>> GetSettingListByGroupAsync(SettingGroup group);
        Task<ConfigurationSetting> GetEntity(long id);
        void AddSetting(ConfigurationSetting entity);
        Task SaveChangesAsync();
    }
}
