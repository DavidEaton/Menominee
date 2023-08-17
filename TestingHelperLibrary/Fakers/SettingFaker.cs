using Bogus;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;

namespace Menominee.TestingHelperLibrary.Fakers;

public class SettingFaker : Faker<ConfigurationSetting>
{
    public SettingFaker(bool generateId = false)
    {
        RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);
        GenerateSetting();
    }

    public SettingFaker(long id = 0)
    {
        RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);
        GenerateSetting();
    }

    private void GenerateSetting()
    {
        CustomInstantiator(faker =>
        {
            var settingName = faker.PickRandom<SettingName>();
            var settingGroup = faker.PickRandom<SettingGroup>();
            var value = faker.Random.Replace("?????????????????");
            var type = SettingHelper.GetType(value);

            var result = ConfigurationSetting.Create(settingName, settingGroup, type, value);

            return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
        });
    }
}
