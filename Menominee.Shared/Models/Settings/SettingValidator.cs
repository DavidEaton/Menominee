using FluentValidation;
using Menominee.Domain.Entities.Settings;

namespace Menominee.Shared.Models.Settings;

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
