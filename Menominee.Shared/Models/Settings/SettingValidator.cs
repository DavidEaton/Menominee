using FluentValidation;
using Menominee.Domain.Entities.Settings;

namespace Menominee.Shared.Models.Settings;

public class SettingValidator : AbstractValidator<SettingToWrite>
{
    private const string notEmptyMessage = "Setting must not be empty";

    public SettingValidator()
    {
        RuleFor(setting => setting.SettingName)
            .NotEmpty()
            .WithMessage(notEmptyMessage)
            .IsInEnum();

        RuleFor(setting => setting.SettingGroup)
            .NotEmpty()
            .WithMessage(notEmptyMessage)
            .IsInEnum();

        RuleFor(setting => setting.SettingValue)
            .NotNull()
            .WithMessage(notEmptyMessage);

        RuleFor(setting => setting)
            .MustBeEntity(setting => ConfigurationSetting.Create(
                setting.SettingName,
                setting.SettingGroup,
                SettingHelper.GetType(setting.SettingValue),
                setting.SettingValue));
    }
}
