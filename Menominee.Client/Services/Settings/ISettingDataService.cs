using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;

namespace Menominee.Client.Services.Settings;

public interface ISettingDataService
{
    Task<Result<SettingToRead?>> GetAsync(SettingName name);
    Task<Result<IReadOnlyList<SettingToRead?>>> GetAllAsync();
    Task<Result<IReadOnlyList<SettingToRead?>>> GetByGroupAsync(SettingGroup group);
    Task<Result<PostResponse>> AddAsync(SettingToWrite setting);
    Task<Result> AddMultipleAsync(IReadOnlyList<SettingToWrite> settings);
}