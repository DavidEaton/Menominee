using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Settings;

public class SettingDataService : ISettingDataService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<PersonDataService> logger;
    private const string MediaType = "application/json";
    private const string UriSegment = "api/settings";

    public SettingDataService(HttpClient httpClient, ILogger<PersonDataService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SettingToRead?> GetSetting(SettingName name)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<SettingToRead>($"{UriSegment}/{name}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get setting {name}", name);
        }

        return null;
    }

    public async Task<IReadOnlyList<SettingToRead>?> GetSettingsList()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<IReadOnlyList<SettingToRead>>(UriSegment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get all settings");
        }

        return null;
    }

    public async Task<IReadOnlyList<SettingToRead>?> GetSettingsList(SettingGroup group)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<IReadOnlyList<SettingToRead>>($"{UriSegment}/group/{group}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get settings for group {group}", group);
        }

        return null;
    }

    public async Task<SettingToRead?> SaveSetting(SettingToWrite setting)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(UriSegment, setting);

            return await response.Content.ReadFromJsonAsync<SettingToRead>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save setting {setting}", setting.SettingName);
        }

        return null;
    }

    public async Task<IReadOnlyList<SettingToRead>?> SaveSettingsList(IReadOnlyList<SettingToWrite> settings)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{UriSegment}/settingList", settings);

            return await response.Content.ReadFromJsonAsync<IReadOnlyList<SettingToRead>>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save settings");
        }

        return null;
    }
}
