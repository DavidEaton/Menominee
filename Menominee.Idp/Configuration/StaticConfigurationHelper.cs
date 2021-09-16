using Microsoft.Extensions.Configuration;
using System.IO;

namespace Menominee.Idp.Configuration
{
    public class StaticConfigurationHelper
    {
        private static StaticConfigurationHelper appSettings;

        public string appSettingValue { get; set; }

        public static string AppSetting(string Key)
        {
            appSettings = GetCurrentSettings(Key);
            return appSettings.appSettingValue;
        }

        public StaticConfigurationHelper(IConfiguration config, string Key)
        {
            appSettingValue = config.GetValue<string>(Key);
        }

        // Get a valued stored in the appsettings.
        // Pass in a key like TestArea:TestKey to get TestValue
        public static StaticConfigurationHelper GetCurrentSettings(string Key)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new StaticConfigurationHelper(configuration.GetSection("ApplicationSettings"), Key);

            return settings;
        }
    }
}
