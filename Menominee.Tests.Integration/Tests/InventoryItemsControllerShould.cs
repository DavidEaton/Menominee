using Bogus;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Menominee.Api.Data;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;
using Menominee.Shared.Models.Inventory.InventoryItems.Tire;
using Menominee.Shared.Models.Inventory.InventoryItems.Warranty;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.Taxes;
using Menominee.Tests.Helpers;
using Menominee.Tests.Helpers.Fakers;
using Menominee.Tests.Integration.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
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
        private readonly Faker faker;
        public InventoryItemsControllerShould(IntegrationTestWebApplicationFactory factory)
        {
            httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost/api/")
            });

            faker = new Faker();
            dataSeeder = factory.Services.GetRequiredService<IDataSeeder>();
            dbContext = factory.Services.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

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
        }

        [Fact]
        public async Task Return_Failure_On_Add_an_inventory_item_with_null_Manufacturer()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();
            inventoryItemToPost.Manufacturer = null;

            var result = await PostInventoryItem(inventoryItemToPost);

            result.Should().Contain("Error: NotFound");
        }

        [Fact]
        public async Task Return_Failure_On_Add_an_inventory_item_with_null_ProductCode()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();
            inventoryItemToPost.ProductCode = null;

            var result = await PostInventoryItem(inventoryItemToPost);

            result.Should().Contain("Error: NotFound");
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
        public async Task Update_an_InventoryItem_Part()
        {
            var itemToUpdate = dbContext.InventoryItems.First();
            var originalId = itemToUpdate.Part.Id;
            var originalList = itemToUpdate.Part.List;
            var originalCost = itemToUpdate.Part.Cost;
            var originalCore = itemToUpdate.Part.Core;
            var originalRetail = itemToUpdate.Part.Retail;
            var originalLineCode = itemToUpdate.Part.LineCode;
            var originalSubLineCode = itemToUpdate.Part.SubLineCode;
            var originalFractional = itemToUpdate.Part.Fractional;
            var originalExciseFees = CreateOriginalExciseFees(itemToUpdate.Part.ExciseFees);
            var updatedAmount = Math.Round(faker.Random.Double(0, 99), 2);
            var updatedTechAmount = TechAmount.Create(ItemLaborType.Flat, updatedAmount, SkillLevel.A).Value;
            var updatedLineCode = faker.Random.AlphaNumeric(10).ToUpper();
            var updatedSubLineCode = faker.Random.AlphaNumeric(10).ToLower();
            var updatedFractional = !originalFractional;
            var updatedExciseFees = UpdateExciseFees(itemToUpdate.Part.ExciseFees, updatedAmount);
            var updatedDescription = "Updated by integration test";
            var updatedFeeType = ExciseFeeType.Percentage;
            itemToUpdate.Part.SetCore(updatedAmount);
            itemToUpdate.Part.SetCost(updatedAmount);
            itemToUpdate.Part.SetList(updatedAmount);
            itemToUpdate.Part.SetRetail(updatedAmount);
            itemToUpdate.Part.SetTechAmount(updatedTechAmount);
            itemToUpdate.Part.SetLineCode(updatedLineCode);
            itemToUpdate.Part.SetSubLineCode(updatedSubLineCode);
            itemToUpdate.Part.SetFractional(updatedFractional);
            itemToUpdate.Part.UpdateExciseFees(updatedExciseFees);
            var updatedInventoryItem = new InventoryItemToWrite()
            {
                Id = itemToUpdate.Id,
                Manufacturer = ManufacturerHelper.ConvertToWriteDto(itemToUpdate.Manufacturer),
                ItemNumber = itemToUpdate.ItemNumber,
                Description = itemToUpdate.Description,
                ProductCode = ProductCodeHelper.ConvertToWriteDto(itemToUpdate.ProductCode),
                ItemType = itemToUpdate.ItemType,

                Part = InventoryItemPartHelper.ConvertToWriteDto(itemToUpdate.Part),
            };

            var response = await httpClient.PutAsync($"{route}/{itemToUpdate.Id}", JsonContent.Create(updatedInventoryItem));

            response.EnsureSuccessStatusCode();
            var itemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemToUpdate.Id}");
            itemFromEndpoint.Should().NotBeNull();
            itemFromEndpoint.Part.Id.Should().Be(originalId);
            itemFromEndpoint.Part.List.Should().NotBe(originalList);
            itemFromEndpoint.Part.Cost.Should().NotBe(originalCost);
            itemFromEndpoint.Part.Core.Should().NotBe(originalCore);
            itemFromEndpoint.Part.Retail.Should().NotBe(originalRetail);
            itemFromEndpoint.Part.LineCode.Should().NotBe(originalLineCode);
            itemFromEndpoint.Part.SubLineCode.Should().NotBe(originalSubLineCode);
            itemFromEndpoint.Part.Fractional.Should().NotBe(originalFractional);
            itemFromEndpoint.Part.List.Should().Be(updatedAmount);
            itemFromEndpoint.Part.Core.Should().Be(updatedAmount);
            itemFromEndpoint.Part.Cost.Should().Be(updatedAmount);
            itemFromEndpoint.Part.Retail.Should().Be(updatedAmount);
            itemFromEndpoint.Part.LineCode.Should().Be(updatedLineCode);
            itemFromEndpoint.Part.SubLineCode.Should().Be(updatedSubLineCode);
            itemFromEndpoint.Part.Fractional.Should().Be(updatedFractional);
            foreach (var updatedFee in itemFromEndpoint.Part.ExciseFees)
            {
                var originalFee = originalExciseFees.FirstOrDefault(f => f.Id == updatedFee.Id);

                if (originalFee is not null)
                {
                    originalFee.Amount.Should().NotBe(updatedFee.Amount);
                    originalFee.Description.Should().NotBe(updatedFee.Description);
                    originalFee.FeeType.Should().NotBe(null);

                    updatedFee.Amount.Should().Be(updatedAmount);
                    updatedFee.Description.Should().Be(updatedDescription);
                    updatedFee.FeeType.Should().Be(updatedFeeType);
                }
            }
        }

        private List<ExciseFeeToRead> CreateOriginalExciseFees(IReadOnlyList<ExciseFee> exciseFees)
        {
            return exciseFees.Select(fee => new ExciseFeeToRead
            {
                Id = fee.Id,
                Description = fee.Description,
                FeeType = fee.FeeType,
                Amount = fee.Amount
            }).ToList();
        }

        private IReadOnlyList<ExciseFee> UpdateExciseFees(IReadOnlyList<ExciseFee> exciseFees, double updatedAmount)
        {
            return exciseFees.Select(fee =>
            {
                var amountResult = fee.SetAmount(updatedAmount);
                var descriptionResult = fee.SetDescription("Updated by integration test");
                var feeTypeResult = fee.SetFeeType(ExciseFeeType.Percentage);

                var combinedResult = Result.Combine(amountResult, descriptionResult, feeTypeResult);

                return combinedResult.IsSuccess
                    ? fee
                    : throw new Exception("Failed to update fee");
            }).ToList();
        }


        //[Fact]
        //public async Task Get_an_InventoryItem_by_Manufacturer_Id_and_Item_Number()
        //{
        //    var itemFromDatabase = dbContext.InventoryItems.First();

        //    var itemFromEndpoint = await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{route}/{itemFromDatabase.Id}");

        //    itemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        //    true.Should().BeFalse();
        //}

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
            var collectionCount = 3;
            var generateId = false;
            var inventoryItems = new InventoryItemFaker(generateId, collectionCount).Generate(count);

            dataSeeder.Save(inventoryItems);
        }

        public void Dispose()
        {
            dbContext.Manufacturers.RemoveRange(dbContext.Manufacturers.ToList());
            dbContext.SaleCodeShopSupplies.RemoveRange(dbContext.SaleCodeShopSupplies.ToList());
            dbContext.SaleCodes.RemoveRange(dbContext.SaleCodes.ToList());
            dbContext.ProductCodes.RemoveRange(dbContext.ProductCodes.ToList());
            dbContext.InventoryItemParts.RemoveRange(dbContext.InventoryItemParts.ToList());
            dbContext.InventoryItems.RemoveRange(dbContext.InventoryItems.ToList());
            dbContext.ExciseFees.RemoveRange(dbContext.ExciseFees.ToList());

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }
    }
}
