using FluentValidation;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;

namespace Menominee.Api.Features.Settings;

public class SettingValidator : AbstractValidator<SettingToWrite>
{
    public SettingValidator()
    {
        RuleFor(setting => setting)
            .MustBeEntity(setting => ConfigurationSetting.Create(
                setting.SettingName,
                setting.SettingGroup,
                SettingHelper.GetType(setting.SettingValue),
                setting.SettingValue));
    }
}
