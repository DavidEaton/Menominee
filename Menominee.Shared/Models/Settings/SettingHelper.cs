using Menominee.Domain.Entities.Settings;
using System.Collections.Generic;
using System;

namespace Menominee.Shared.Models.Settings
{
    public class SettingHelper
    {
        /// <summary>
        /// Converts a SettingConfiguration Entity to a SettingToRead Dto
        /// </summary>
        /// <param name="setting">ConfigurationSetting Entity</param>
        /// <returns>SettingToRead Dto</returns>
        public static SettingToRead ConvertToReadDto(ConfigurationSetting setting)
        {
            if(setting is null)
                return null;

            var settingToRead = new SettingToRead
            {
                Id = setting.Id,
                SettingName = setting.SettingName,
                SettingValueType = setting.SettingValueType,
                SettingValue = setting.Value,
                SettingGroup = setting.SettingGroup
            };

            return settingToRead;
        }

        /// <summary>
        /// Converts a WriteDto into a ReadDto
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>SettingToReadDto</returns>
        public static SettingToRead ConvertWriteToReadDto(SettingToWrite setting)
        {
            if (setting is null)
                return null;

            var settingToRead = new SettingToRead
            {
                SettingName = setting.SettingName,
                SettingValue = setting.SettingValue,
                SettingGroup = setting.SettingGroup
            };

            return settingToRead;
        }

        /// <summary>
        /// Converts a WriteDto into a Entity and Sets the Type based on the values type
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>EnfigurationSetting Entity</returns>
        public static ConfigurationSetting ConvertWriteDtoToEntity(SettingToWrite setting)
        {
            return setting is null
                ? null
                : ConfigurationSetting.Create(
                    setting.SettingName,
                    setting.SettingGroup,
                    GetType(setting.SettingValue),
                    setting.SettingValue)
                .Value;
        }

        /// <summary>
        /// Determines the type of a value
        /// </summary>
        /// <param name="type">string value of type to be determined</param>
        /// <returns>Type as name as a string</returns>
        public static string GetType(string value)
        {
            var parsers = new List<(Func<string, bool> parser, string typeName)>
            {
                ((string value) => bool.TryParse(value, out _), nameof(Boolean)),
                ((string value) => decimal.TryParse(value, out _), nameof(Decimal)),
                ((string value) => int.TryParse(value, out _), nameof(Int32))
            };
            foreach (var (parser, typeName) in parsers)
                if (parser(value))
                    return typeName;

            return value.GetType().Name;
        }
    }
}
