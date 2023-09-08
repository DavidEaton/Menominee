using FluentAssertions;
using Menominee.Shared.Models.SellingPriceNames;
using Menominee.TestingHelperLibrary.Fakers;
using Menominee.Tests.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

public class SellingPriceNamesControllerShould : IntegrationTestBase
{
    private const string route = "sellingpricenames";
    public SellingPriceNamesControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Route_Returns_NotFound()
    {
        var response = await httpClient.GetAsync("invalid-route");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await httpClient.GetAsync($"{route}/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Id_Returns_SellingPriceName()
    {
        var sellingPriceName = dbContext.SellingPriceNames.First();
        var response = await httpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{route}/{sellingPriceName.Id}");

        response.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName));
    }

    [Fact]
    public async Task Add_SellingPriceName()
    {
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();
        var sellingPriceNameToAdd = SellingPriceNameHelper.ConvertToWriteDto(sellingPriceName);

        var result = await httpClient.PostAsJsonAsync(route, sellingPriceNameToAdd);

        var id = (await result.Content.ReadFromJsonAsync<SellingPriceNameToRead>()).Id;
        var sellingPriceNameFromEndpoint = await httpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{route}/{id}");

        sellingPriceNameFromEndpoint.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName), options => options.Excluding(s => s.Id));
    }

    [Fact]
    public async Task Update_SellingPriceName()
    {
        var sellingPriceNameFromEndpoint = dbContext.SellingPriceNames.First();
        var sellingPriceName = new SellingPriceNameFaker(sellingPriceNameFromEndpoint.Id).Generate();
        var sellingPriceNameToUpdate = SellingPriceNameHelper.ConvertToWriteDto(sellingPriceName);

        var result = await httpClient.PutAsJsonAsync($"{route}/{sellingPriceNameToUpdate.Id}", sellingPriceNameToUpdate);
        result.EnsureSuccessStatusCode();

        var sellingPriceNameFromEndpointAfterUpdate = await httpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{route}/{sellingPriceNameToUpdate.Id}");

        sellingPriceNameFromEndpointAfterUpdate.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName));
    }

    [Fact]
    public async Task Delete_SellingPriceName()
    {
        var sellingPriceNameToDelete = dbContext.SellingPriceNames.First();

        sellingPriceNameToDelete.Should().NotBeNull();

        var response = await httpClient.DeleteAsync($"{route}/{sellingPriceNameToDelete.Id}");
        response.EnsureSuccessStatusCode();

        var sellingPriceNameFromEndpoint = await httpClient.GetAsync($"{route}/{sellingPriceNameToDelete.Id}");

        sellingPriceNameFromEndpoint.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public override void SeedData()
    {
        var count = 2;
        var sellingPriceNames = new SellingPriceNameFaker(false).Generate(count);

        dataSeeder.Save(sellingPriceNames);
    }

    public override void Dispose()
    {
        dbContext.RemoveRange(dbContext.SellingPriceNames.ToList());
        DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
    }
}
