using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Settings
{
    public interface ISettingsRepository
    {
        Task<ActionResult<IReadOnlyList<SettingToRead>>> GetSettingsListAsync();
        Task<ActionResult<SettingToRead>> GetSetting(SettingName settingName);
        Task<ActionResult<IReadOnlyList<SettingToRead>>> SaveSettingsListAsync(List<SettingToWrite> settings);
        Task<ActionResult<SettingToRead>> SaveSetting(SettingToWrite settingToWrite);
        Task<ActionResult<IReadOnlyList<SettingToRead>>> UpdateSettingsListAsync(List<SettingToWrite> settings);
        Task<ActionResult<SettingToRead>> UpdateSetting(SettingToWrite setting);
        Task<ActionResult<IReadOnlyList<SettingToRead>>> GetSettingListByGroupAsync(SettingGroup group);
        Task SaveChangesAsync();
    }
}
