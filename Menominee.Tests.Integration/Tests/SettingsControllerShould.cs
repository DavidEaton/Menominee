﻿using FluentAssertions;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using Menominee.TestingHelperLibrary.Fakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

[Collection("Integration")]
public class SettingsControllerShould : IntegrationTestBase
{
    private const string Route = "settings";

    public SettingsControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_SettingName_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}/settingName/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Returns_Expected_Response()
    {
        var settingFromDatabase = DbContext.Settings.First();

        var response = await HttpClient.GetFromJsonAsync<SettingToRead>($"{Route}/{settingFromDatabase.SettingName}");

        response.Should().BeOfType<SettingToRead>();
    }

    [Fact]
    public async Task Save_a_Setting()
    {
        var settingToPost = new SettingFaker(false).Generate();
        var settingNameToPost = PickRandomUniqueSettingName().First();
        settingToPost.SetSettingName(settingNameToPost);

        var settingToWrite = SettingHelper.ConvertToWriteDto(settingToPost);

        var result = await HttpClient.PostAsJsonAsync(Route, settingToWrite);

        var settingFromEndpoint = await HttpClient
            .GetFromJsonAsync<SettingToRead>($"{Route}/{settingNameToPost}");

        settingFromEndpoint.Should().BeOfType<SettingToRead>();
        settingFromEndpoint.Should()
            .BeEquivalentTo(SettingHelper.ConvertToReadDto(settingToPost), options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Save_a_Settings_List()
    {
        var count = 2;
        var settingsListToPost = new SettingFaker(false).Generate(count);
        var existingSettingNames = DbContext.Settings.Select(x => x.SettingName).ToList();
        var settingNamesToPost = PickRandomUniqueSettingName(count);

        settingsListToPost.ForEach(setting =>
        {
            var index = settingsListToPost.IndexOf(setting);
            setting.SetSettingName(settingNamesToPost[index]);
        });

        var result = await HttpClient.PostAsJsonAsync($"{Route}/settingList", settingsListToPost.Select(setting => SettingHelper.ConvertToWriteDto(setting)).ToList());
        result.EnsureSuccessStatusCode();

        var settingsListFromEndpoint = await HttpClient
            .GetFromJsonAsync<IReadOnlyList<SettingToRead>>(Route);

        settingsListFromEndpoint.Should().NotBeEmpty();
        settingsListFromEndpoint.Should().BeOfType<List<SettingToRead>>();
        settingsListFromEndpoint.Should().ContainEquivalentOf(SettingHelper.ConvertToReadDto(settingsListToPost[0]), options => options.Excluding(x => x.Id));
        settingsListFromEndpoint.Should().ContainEquivalentOf(SettingHelper.ConvertToReadDto(settingsListToPost[1]), options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Update_a_Setting()
    {
        var settingToUpdate = DbContext.Settings.First();
        var updatedSetting = new SettingFaker(settingToUpdate.Id).Generate();
        updatedSetting.SetSettingName(settingToUpdate.SettingName);
        updatedSetting.SetSettingGroup(settingToUpdate.SettingGroup);

        var settingToWrite = SettingHelper.ConvertToWriteDto(updatedSetting);
        var response = await HttpClient.PutAsJsonAsync(Route, settingToWrite);
        response.EnsureSuccessStatusCode();

        var settingFromEndpoint = await HttpClient
            .GetFromJsonAsync<SettingToRead>($"{Route}/{settingToUpdate.SettingName}");

        settingFromEndpoint.Should().NotBeNull();
        settingFromEndpoint.Should().BeOfType<SettingToRead>();
        settingFromEndpoint.Should()
            .BeEquivalentTo(SettingHelper.ConvertToReadDto(updatedSetting));
    }

    [Fact]
    public async Task Update_a_Settings_List()
    {
        var settingsListToUpdate = DbContext.Settings.ToList();
        var updatedSettingsList = settingsListToUpdate
            .Select(setting => new SettingFaker(setting.Id).Generate())
            .ToList();

        updatedSettingsList.ForEach(setting =>
        {
            var index = updatedSettingsList.IndexOf(setting);
            setting.SetSettingName(settingsListToUpdate[index].SettingName);
            setting.SetSettingGroup(settingsListToUpdate[index].SettingGroup);
        });

        var response = await HttpClient.PutAsJsonAsync($"{Route}/settingList", updatedSettingsList.Select(setting => SettingHelper.ConvertToWriteDto(setting)).ToList());
        response.EnsureSuccessStatusCode();

        var settingsListFromEndpoint = await HttpClient
            .GetFromJsonAsync<IReadOnlyList<SettingToRead>>(Route);

        settingsListFromEndpoint.Should().NotBeEmpty();
        settingsListFromEndpoint.Should().BeOfType<List<SettingToRead>>();
        settingsListFromEndpoint[0].Should()
            .BeEquivalentTo(SettingHelper.ConvertToReadDto(updatedSettingsList[0]));
        settingsListFromEndpoint[1].Should()
            .BeEquivalentTo(SettingHelper.ConvertToReadDto(updatedSettingsList[1]));
    }

    private async Task<string> PostSetting(SettingToWrite setting)
    {
        {
            var json = JsonSerializer.Serialize(setting, JsonSerializerHelper.DefaultSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.PostAsync(Route, content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var errorContent = await response.Content.ReadAsStringAsync();

            var (success, apiError) = JsonSerializerHelper.DeserializeApiError(errorContent);

            return success
                ? $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}"
                : throw new JsonException("Failed to deserialize ApiError");
        }
    }

    public static int GetSettingNameFromString(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var settingName = root.GetProperty("settingName").GetInt32();
        return settingName;
    }

    public List<SettingName> PickRandomUniqueSettingName(int listLength = 1)
    {
        var existingSettingNames = DbContext.Settings.Select(x => x.SettingName).ToList();
        var allSettings = Enum.GetValues(typeof(SettingName)).Cast<SettingName>().ToList();
        var availableSettings = allSettings.Where(s => !existingSettingNames.Contains(s)).ToList();
        var randomizer = new Random();

        var randomSettings = availableSettings.OrderBy(x => randomizer.Next()).Take(listLength);

        return randomSettings.Count() == listLength
            ? randomSettings.ToList()
            : default;
    }

    public override void SeedData()
    {
        var count = 2;
        var settings = new SettingFaker(false).Generate(count);
        var settingNames = PickRandomUniqueSettingName(count);
        settings.ForEach(setting =>
        {
            var index = settings.IndexOf(setting);
            setting.SetSettingName(settingNames[index]);
        });

        DataSeeder.Save(settings);
    }
}
