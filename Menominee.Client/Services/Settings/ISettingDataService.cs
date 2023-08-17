using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;

namespace Menominee.Client.Services.Settings;

public interface ISettingDataService
{
    Task<SettingToRead?> GetSetting(SettingName name);
    Task<IReadOnlyList<SettingToRead>?> GetSettingsList();
    Task<IReadOnlyList<SettingToRead>?> GetSettingsList(SettingGroup group);
    Task<SettingToRead?> SaveSetting(SettingToWrite setting);
    Task<IReadOnlyList<SettingToRead>?> SaveSettingsList(IReadOnlyList<SettingToWrite> settings);
}
