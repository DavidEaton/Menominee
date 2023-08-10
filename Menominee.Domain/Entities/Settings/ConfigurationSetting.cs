using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Settings
{
    public class ConfigurationSetting : Entity
    {
        #region Variables
        public const string RequiredMessage = "A valid value is required.";
        public static readonly string SettingNameInvalidMessage = $"Please enter a valid Setting Name";
        public static readonly string SettingGroupInvalidMessage = $"Please enter a valid Setting Group";

        #endregion
        public SettingName SettingName { get; private set; }
        public SettingGroup SettingGroup { get; private set; }
        public string SettingValueType { get; private set; }
        public string Value { get; private set; }

        private ConfigurationSetting(SettingName settingName, SettingGroup settingGroup, string type, string value) 
        {
            SettingName = settingName;
            SettingGroup = settingGroup;
            SettingValueType = type;
            Value = value;
        }

        public static Result<ConfigurationSetting> Create(SettingName settingName, SettingGroup settingGroup, string type, string value)
        {
            value = (value ?? string.Empty).Trim();
            // parse each parameter
            if (!Enum.IsDefined(typeof(SettingName), settingName))
                return Result.Failure<ConfigurationSetting>(SettingNameInvalidMessage);

            if(!Enum.IsDefined(typeof(SettingGroup), settingGroup))
                return Result.Failure<ConfigurationSetting>(SettingGroupInvalidMessage);

            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<ConfigurationSetting>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(type))
                return Result.Failure<ConfigurationSetting>(RequiredMessage);

            return Result.Success(new ConfigurationSetting(settingName, settingGroup, type, value));
        }

        public Result UpdateSettingProperties(SettingName settingName, SettingGroup settingGroup, string type, string value)
        {
            return Result.Combine(
                (settingName != SettingName)
                    ? SetSettingName(settingName)
                    : Result.Success(),
                (settingGroup != SettingGroup)
                    ? SetSettingGroup(settingGroup)
                    : Result.Success(),
                (type is not null) && (type != SettingValueType)
                    ? SetValueType(type)
                    : Result.Success(),
                (value is not null) && (value != Value)
                    ? SetSettingValue(value)
                    : Result.Success());
        }

        public Result<SettingName> SetSettingName(SettingName name)
        {
            return
                !Enum.IsDefined(typeof(SettingName), name)
                ? Result.Failure<SettingName>(SettingNameInvalidMessage)
                : Result.Success(SettingName = name);
        }

        public Result<string> SetSettingValue(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<string>(RequiredMessage);

            value = (value ?? string.Empty).Trim();

            return Result.Success(Value = value);
        }

        public Result<string> SetValueType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return Result.Failure<string>(RequiredMessage);

            type = (type ?? string.Empty).Trim();

            return Result.Success(SettingValueType = type);
        }

        public Result<SettingGroup> SetSettingGroup(SettingGroup settingGroup)
        {
            return 
                !Enum.IsDefined(typeof(SettingGroup), settingGroup)
                ? Result.Failure<SettingGroup>(SettingGroupInvalidMessage) 
                : Result.Success(SettingGroup = settingGroup);
        }

        #region ORM
        // EF requires a parameterless constructor
        protected ConfigurationSetting() { }

        #endregion

    }
}
