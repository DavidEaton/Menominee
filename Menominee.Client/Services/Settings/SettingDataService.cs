using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Settings;

public class SettingDataService : DataServiceBase<SettingDataService>, ISettingDataService
{
    private readonly HttpClient httpClient;
    private const string UriSegment = "api/settings";

    public SettingDataService(HttpClient httpClient,
        ILogger<SettingDataService> logger,
        UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<SettingToRead?>> GetAsync(SettingName name)
    {
        var errorMessage = $"Failed to get excise fee with name {name}";

        try
        {
            var result = await httpClient.GetFromJsonAsync<SettingToRead?>(UriSegment + $"/{name}");
            return result is not null
                ? Result.Success(result)
                : Result.Failure<SettingToRead?>(errorMessage);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, errorMessage);
            return Result.Failure<SettingToRead?>(errorMessage);
        }
    }

    public async Task<Result<IReadOnlyList<SettingToRead?>>> GetAllAsync()
    {
        var errorMessage = "Failed to get all businesses";

        try
        {
            var result = await httpClient.GetFromJsonAsync<IReadOnlyList<SettingToRead?>>(UriSegment);
            return result is not null
                ? Result.Success(result)
                : Result.Failure<IReadOnlyList<SettingToRead?>>(errorMessage);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, errorMessage);
            return Result.Failure<IReadOnlyList<SettingToRead?>>(errorMessage);
        }
    }

    public async Task<Result<IReadOnlyList<SettingToRead?>>> GetByGroupAsync(SettingGroup group)
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<IReadOnlyList<SettingToRead>>($"{UriSegment}/group/{group}");

            if (result is not null)
                return Result.Success(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to get settings for group {group}", group);
        }

        return Result.Failure<IReadOnlyList<SettingToRead?>>($"Failed to get setting {group}");
    }

    public async Task<Result<PostResponse>> AddAsync(SettingToWrite fromCaller)
    {
        var entityType = "Setting";
        try
        {
            var result = await httpClient.AddAsync(
                UriSegment,
                fromCaller,
                Logger);

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Failed to add {entityType}");
            return Result.Failure<PostResponse>("An unexpected error occurred");
        }
    }

    public async Task<Result> AddMultipleAsync(IReadOnlyList<SettingToWrite> settings)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{UriSegment}/settingList", settings);

            if (response.IsSuccessStatusCode)
                return Result.Success();

            return Result.Failure("Failed to add settings");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save settings");
        }

        return Result.Failure("Failed to add settings");
    }
}
