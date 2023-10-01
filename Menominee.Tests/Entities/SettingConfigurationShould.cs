using FluentAssertions;
using Menominee.Domain.Entities.Settings;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class SettingConfigurationShould
    {
        private static ConfigurationSetting CreateConfigurationSetting()
        {
            return ConfigurationSetting.Create(SettingName.DeclineParts, SettingGroup.Prompt, "Boolean", "true").Value;
        }

        [Fact]
        public void Create_Setting()
        {
            // Arrange
            var name = SettingName.DeclineParts;
            var group = SettingGroup.Prompt;
            var type = "Boolean";
            var value = "true";

            // Act
            var result = ConfigurationSetting.Create(name, group, type, value);

            // Assert
            result.Value.Should().BeOfType<ConfigurationSetting>();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Value()
        {
            var name = SettingName.DeclineParts;
            var group = SettingGroup.Prompt;
            var type = "Boolean";
            string value = null;

            var result = ConfigurationSetting.Create(name, group, type, value);

            result.Error.Should().Be(ConfigurationSetting.RequiredMessage);
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_ValueType()
        {
            var name = SettingName.DeclineParts;
            var group = SettingGroup.Prompt;
            string type = null;
            string value = "false";

            var result = ConfigurationSetting.Create(name, group, type, value);

            result.Error.Should().Be(ConfigurationSetting.RequiredMessage);
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_Invalid_Type()
        {
            var name = (SettingName)111;
            var group = SettingGroup.Prompt;
            var type = "Boolean";
            string value = null;

            var result = ConfigurationSetting.Create(name, group, type, value);

            result.Error.Should().Be(ConfigurationSetting.SettingNameInvalidMessage);
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_Invalid_Group()
        {
            var name = SettingName.DeclineParts;
            var group = (SettingGroup)111;
            var type = "Boolean";
            string value = null;

            var result = ConfigurationSetting.Create(name, group, type, value);

            result.Error.Should().Be(ConfigurationSetting.SettingGroupInvalidMessage);
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetSettingName()
        {
            var setting = CreateConfigurationSetting();

            setting.SettingName.Should().Be(SettingName.DeclineParts);
            setting.SetSettingName(SettingName.MonthsToRetain);

            setting.SettingName.Should().Be(SettingName.MonthsToRetain);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Name()
        {
            var setting = CreateConfigurationSetting();

            var invalidName = (SettingName)111;

            setting.SettingName.Should().Be(SettingName.DeclineParts);
            var result = setting.SetSettingName(invalidName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ConfigurationSetting.SettingNameInvalidMessage);
        }

        [Fact]
        public void SetSettingGroup()
        {
            var setting = CreateConfigurationSetting();
            setting.SettingGroup.Should().Be(SettingGroup.Prompt);
            setting.SetSettingGroup(SettingGroup.Security);

            setting.SettingGroup.Should().Be(SettingGroup.Security);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Group()
        {
            var setting = CreateConfigurationSetting();

            var invalidGroup = (SettingGroup)111;

            setting.SettingGroup.Should().Be(SettingGroup.Prompt);
            var result = setting.SetSettingGroup(invalidGroup);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ConfigurationSetting.SettingGroupInvalidMessage);
        }

        [Fact]
        public void SetSettingValueType()
        {
            var setting = CreateConfigurationSetting();

            string newValueType = "Int";

            setting.SettingValueType.Should().Be("Boolean");
            setting.SetValueType(newValueType);

            setting.SettingValueType.Should().Be(newValueType);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Type()
        {
            var setting = CreateConfigurationSetting();

            string invalidType = null;

            setting.SettingValueType.Should().Be("Boolean");
            var result = setting.SetValueType(invalidType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ConfigurationSetting.RequiredMessage);
        }

        [Fact]
        public void SetSettingValue()
        {
            var setting = CreateConfigurationSetting();

            string newValue = "false";

            setting.Value.Should().Be("true");
            setting.SetSettingValue(newValue);

            setting.Value.Should().Be(newValue);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Value()
        {
            var setting = CreateConfigurationSetting();

            string invalidValue = null;

            setting.Value.Should().Be("true");
            var result = setting.SetSettingValue(invalidValue);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ConfigurationSetting.RequiredMessage);
        }

        [Fact]
        public void Update_Settings()
        {
            var name = SettingName.DeclineParts;
            var group = SettingGroup.Prompt;
            var type = "Boolean";
            var value = "true";
            var setting = ConfigurationSetting.Create(name, group, type, value).Value;

            setting.SetProperties(name, group, type, "false");

            setting.Value.Should().Be("false");
        }
    }
}
