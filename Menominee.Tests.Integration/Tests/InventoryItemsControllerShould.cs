using Bogus;
using Menominee.Api.Data;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;
using Menominee.Shared.Models.Inventory.InventoryItems.Tire;
using Menominee.Shared.Models.Inventory.InventoryItems.Warranty;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Tests.Helpers;
using Menominee.Tests.Helpers.Fakers;
using Menominee.Tests.Integration.Data;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;

/*
 * VK: I'll be outlining what I did using "VK" comments. I'll also put importance/priority to my edits, so you know how important that edit is:
 * Importance (Im.) 1: minor issue, borderline matter of taste. You can reverse the edit freely if you prefer the old way
 * Im.2: medium priority, something I'd generally recommend you do
 * Im.3: something very important
 */

namespace Menominee.Tests.Integration.Tests
{
    // TODO: Mock httpClient or test the endpoints directly for CI/CD
    [Collection("Integration")]
    public class InventoryItemsControllerShould : IClassFixture<IntegrationTestWebApplicationFactory>, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IDataSeeder dataSeeder;
        private readonly ApplicationDbContext dbContext;
        private const string route = "inventoryitems";

        public InventoryItemsControllerShould(IntegrationTestWebApplicationFactory factory)
        {
            httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost/api/")
            });

            dataSeeder = factory.Services.GetRequiredService<IDataSeeder>();
            dbContext = factory.Services.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            SeedData();
        }

        [Fact]
        public async Task Get_Invalid_Route_Returns_NotFound()
        {
            var response = await httpClient.GetAsync("inventoryitem");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Invalid_Id_Returns_NotFound()
        {
            var response = await httpClient.GetAsync($"{route}/0");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Returns_Expected_Response()
        {
            var itemFromDatabase = dbContext.InventoryItems.First();

            var inventoryItemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemFromDatabase.Id}");

            inventoryItemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        [Fact]
        public async Task Add_an_inventory_item()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();

            var result = await PostInventoryItem(inventoryItemToPost);

            var id = JsonSerializerHelper.GetIdFromString(result);
            var inventoryItemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{id}");

            inventoryItemFromEndpoint.Should().BeOfType<InventoryItemToRead>();

            // VK Im.1: You can assert against what the API returns in GetInventoryItem, but it's easier to just check
            // the object in the database. Moreover, you don't always have appropriate APIs to peer into the DB, so 
            // asserting against domain objects is often the only choice. Article on this topic: https://enterprisecraftsmanship.com/posts/how-to-assert-database-state/
        }

        [Fact]
        public async Task Get_Valid_Id_Returns_Invoice()
        {
            var itemFromDatabase = dbContext.InventoryItems.First();

            var itemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemFromDatabase.Id}");

            itemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        [Fact]
        public async Task Delete_an_InventoryItem()
        {
            var itemToDelete = dbContext.InventoryItems.First();
            itemToDelete.Should().NotBeNull();

            var response = await httpClient.DeleteAsync($"{route}/{itemToDelete.Id}");

            response.EnsureSuccessStatusCode();
            var deletedInvoiceFromDatabase = dbContext.InventoryItems.FirstOrDefault(item => item.Id == itemToDelete.Id);
            deletedInvoiceFromDatabase.Should().BeNull();
        }

        [Fact]
        public async Task Update_an_InventoryItem()
        {
            var itemToUpdate = dbContext.InventoryItems.First();
            var originalItemNumber = itemToUpdate.ItemNumber;
            var updatedItemNumber = new Faker().Random.AlphaNumeric(10).ToUpper(); ;
            var updatedInventoryItem = new InventoryItemToWrite()
            {
                Id = itemToUpdate.Id,
                Description = itemToUpdate.Description,
                ItemNumber = updatedItemNumber,
                ItemType = itemToUpdate.ItemType,
                Inspection = InventoryItemHelper.ConvertToWriteDto(itemToUpdate.Inspection),
                Labor = InventoryItemHelper.ConvertToWriteDto(itemToUpdate.Labor),
                Manufacturer = ManufacturerHelper.ConvertToWriteDto(itemToUpdate.Manufacturer),
                Package = InventoryItemPackageHelper.ConvertToWriteDto(itemToUpdate.Package),
                Part = InventoryItemPartHelper.ConvertToWriteDto(itemToUpdate.Part),
                ProductCode = ProductCodeHelper.ConvertToWriteDto(itemToUpdate.ProductCode),
                Tire = InventoryItemTireHelper.ConvertToWriteDto(itemToUpdate.Tire),
                Warranty = InventoryItemWarrantyHelper.ConvertToWriteDto(itemToUpdate.Warranty)
            };

            var response = await httpClient.PutAsync($"{route}/{itemToUpdate.Id}", JsonContent.Create(updatedInventoryItem));

            response.EnsureSuccessStatusCode();
            var itemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemToUpdate.Id}");
            itemFromEndpoint.Should().NotBeNull();
            itemFromEndpoint.ItemNumber.Should().NotBe(originalItemNumber);
            itemFromEndpoint.ItemNumber.Should().Be(updatedItemNumber);
        }

        [Fact]
        public async Task Get_an_InventoryItem_by_Manufacturer_Id_and_Item_Number()
        {
            var itemFromDatabase = dbContext.InventoryItems.First();

            var itemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemFromDatabase.Id}");

            itemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        // TODO: implement additional integration tests
        //[Fact]
        //public async Task Not_add_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_update_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_delete_invalid_inventory_item_but_return_Status404NotFound()
        //{

        //}

        //[Fact]
        //public async Task Not_get_invalid_inventory_item_but_return_NotFoundResult()
        //{

        //}

        //[Fact]
        //public async Task Not_get_inventory_item_by_invalid_manufacturer_Id_and_part_number_but_return_NotFoundResult()
        //{
        //}
        private async Task<string> PostInventoryItem(InventoryItemToWrite inventoryItem)
        {
            var json = JsonSerializer.Serialize(inventoryItem, JsonSerializerHelper.DefaultSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var errorContent = await response.Content.ReadAsStringAsync();

            var (success, apiError) = JsonSerializerHelper.DeserializeApiError(errorContent);

            return success
                ? $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}"
                : throw new JsonException("Failed to deserialize ApiError");
        }

        private InventoryItemToWrite CreateInventoryItemToPost() => new()
        {
            Description = new Faker().Commerce.ProductDescription().Truncate(255),
            Manufacturer = ManufacturerHelper.ConvertToReadDto(dbContext.Manufacturers.First()),
            ItemNumber = new Faker().Random.AlphaNumeric(10).ToUpper(),
            ItemType = InventoryItemType.Part,
            ProductCode = ProductCodeHelper.ConvertToReadDto(dbContext.ProductCodes.First()),
            Part = InventoryItemPartHelper.ConvertToWriteDto(dbContext.InventoryItemParts.First())
        };

        private void SeedData()
        {
            var count = 2;
            var generateId = false;
            var saleCodeShopSupplies = new SaleCodeShopSuppliesFaker(generateId).Generate(count);
            var saleCodes = new SaleCodeFaker(generateId).Generate(count);
            var manufacturers = new ManufacturerFaker(generateId).Generate(count);
            var productCodes = new ProductCodeFaker(generateId).Generate(count);
            var parts = new InventoryItemPartFaker(generateId).Generate(count);

            dataSeeder.Save(saleCodeShopSupplies);
            foreach (var saleCode in saleCodes)
                saleCode.SetShopSupplies(saleCodeShopSupplies[0]);
            dataSeeder.Save(saleCodes);

            dataSeeder.Save(manufacturers);
            foreach (var productCode in productCodes)
            {
                productCode.SetManufacturer(
                    manufacturers[new Random().Next(manufacturers.Count)],
                    new string[] { "11" });

                productCode.SetSaleCode(saleCodes[new Random().Next(saleCodes.Count)]);
            }

            dataSeeder.Save(parts);
            dataSeeder.Save(productCodes);

            dataSeeder.Save(
                new InventoryItemFaker(generateId: generateId)
                .Generate(count));
        }

        public void Dispose()
        {
            dbContext.Manufacturers.RemoveRange(dbContext.Manufacturers.ToList());
            dbContext.SaleCodeShopSupplies.RemoveRange(dbContext.SaleCodeShopSupplies.ToList());
            dbContext.SaleCodes.RemoveRange(dbContext.SaleCodes.ToList());
            dbContext.ProductCodes.RemoveRange(dbContext.ProductCodes.ToList());
            dbContext.InventoryItemParts.RemoveRange(dbContext.InventoryItemParts.ToList());
            dbContext.InventoryItems.RemoveRange(dbContext.InventoryItems.ToList());

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }
    }
}
