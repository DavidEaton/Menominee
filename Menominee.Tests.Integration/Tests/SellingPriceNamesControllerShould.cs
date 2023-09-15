using FluentAssertions;
using Menominee.Shared.Models.SellingPriceNames;
using Menominee.TestingHelperLibrary.Fakers;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

public class SellingPriceNamesControllerShould : IntegrationTestBase
{
    private const string Route = "sellingpricenames";
    public SellingPriceNamesControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Route_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync("invalid-route");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Id_Returns_SellingPriceName()
    {
        var sellingPriceName = DbContext.SellingPriceNames.First();
        var response = await HttpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{Route}/{sellingPriceName.Id}");

        response.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName));
    }

    [Fact]
    public async Task Add_SellingPriceName()
    {
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();
        var sellingPriceNameToAdd = SellingPriceNameHelper.ConvertToWriteDto(sellingPriceName);

        var result = await HttpClient.PostAsJsonAsync(Route, sellingPriceNameToAdd);

        var id = (await result.Content.ReadFromJsonAsync<SellingPriceNameToRead>()).Id;
        var sellingPriceNameFromEndpoint = await HttpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{Route}/{id}");

        sellingPriceNameFromEndpoint.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName), options => options.Excluding(s => s.Id));
    }

    [Fact]
    public async Task Update_SellingPriceName()
    {
        var sellingPriceNameFromEndpoint = DbContext.SellingPriceNames.First();
        var sellingPriceName = new SellingPriceNameFaker(sellingPriceNameFromEndpoint.Id).Generate();
        var sellingPriceNameToUpdate = SellingPriceNameHelper.ConvertToWriteDto(sellingPriceName);

        var result = await HttpClient.PutAsJsonAsync($"{Route}/{sellingPriceNameToUpdate.Id}", sellingPriceNameToUpdate);
        result.EnsureSuccessStatusCode();

        var sellingPriceNameFromEndpointAfterUpdate = await HttpClient.GetFromJsonAsync<SellingPriceNameToRead>($"{Route}/{sellingPriceNameToUpdate.Id}");

        sellingPriceNameFromEndpointAfterUpdate.Should().BeEquivalentTo(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName));
    }

    [Fact]
    public async Task Delete_SellingPriceName()
    {
        var sellingPriceNameToDelete = DbContext.SellingPriceNames.First();

        sellingPriceNameToDelete.Should().NotBeNull();

        var response = await HttpClient.DeleteAsync($"{Route}/{sellingPriceNameToDelete.Id}");
        response.EnsureSuccessStatusCode();

        var sellingPriceNameFromEndpoint = await HttpClient.GetAsync($"{Route}/{sellingPriceNameToDelete.Id}");

        sellingPriceNameFromEndpoint.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public override void SeedData()
    {
        var count = 2;
        var sellingPriceNames = new SellingPriceNameFaker(false).Generate(count);

        DataSeeder.Save(sellingPriceNames);
    }
}
