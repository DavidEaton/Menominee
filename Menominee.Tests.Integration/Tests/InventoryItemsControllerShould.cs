using Bogus;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;
using Menominee.Domain.Extensions;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Inspection;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;
using Menominee.Shared.Models.Inventory.InventoryItems.Tire;
using Menominee.Shared.Models.Inventory.InventoryItems.Warranty;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.Taxes;
using Menominee.TestingHelperLibrary.Fakers;
using Menominee.Tests.Helpers.Fakers;
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
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Integration.Tests
{
    [Collection("Integration")]
    public class InventoryItemsControllerShould : IntegrationTestBase
    {
        private const string Route = "inventoryitems";
        private readonly Faker Faker = new();

        public InventoryItemsControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Invalid_Route_Returns_NotFound()
        {
            var response = await HttpClient.GetAsync("invalid-route");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Invalid_Id_Returns_NotFound()
        {
            var response = await HttpClient.GetAsync($"{Route}/0");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Returns_Expected_Response()
        {
            var itemFromDatabase = DbContext.InventoryItems.First();

            var inventoryItemFromEndpoint = await HttpClient.GetFromJsonAsync<InventoryItemToRead>($"{Route}/{itemFromDatabase.Id}");

            inventoryItemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Add_an_inventory_item()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();

            var result = await PostInventoryItem(inventoryItemToPost);

            var id = JsonSerializerHelper.GetIdFromString(result);
            var inventoryItemFromEndpoint = await HttpClient.GetFromJsonAsync<InventoryItemToRead>($"{Route}/{id}");

            inventoryItemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Return_Failure_On_Add_an_inventory_item_with_null_Manufacturer()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();
            inventoryItemToPost.Manufacturer = null;

            var result = await PostInventoryItem(inventoryItemToPost);

            result.Should().Contain("Error: NotFound");
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Return_Failure_On_Add_an_inventory_item_with_null_ProductCode()
        {
            var inventoryItemToPost = CreateInventoryItemToPost();
            inventoryItemToPost.ProductCode = null;

            var result = await PostInventoryItem(inventoryItemToPost);

            result.Should().Contain("Error: NotFound");
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Valid_Id_Returns_Invoice()
        {
            var itemFromDatabase = DbContext.InventoryItems.First();

            var itemFromEndpoint = await HttpClient.GetFromJsonAsync<InventoryItemToRead>($"{Route}/{itemFromDatabase.Id}");

            itemFromEndpoint.Should().BeOfType<InventoryItemToRead>();
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Delete_an_InventoryItem()
        {
            var itemToDelete = DbContext.InventoryItems.First();
            itemToDelete.Should().NotBeNull();

            var response = await HttpClient.DeleteAsync($"{Route}/{itemToDelete.Id}");

            response.EnsureSuccessStatusCode();
            var deletedInvoiceFromDatabase = DbContext.InventoryItems.FirstOrDefault(item => item.Id == itemToDelete.Id);
            deletedInvoiceFromDatabase.Should().BeNull();
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_an_InventoryItem()
        {
            var itemToUpdate = DbContext.InventoryItems.First();
            var originalItemNumber = itemToUpdate.ItemNumber;
            var updatedItemNumber = new Faker().Random.AlphaNumeric(10).ToUpper(); ;
            var updatedInventoryItem = new InventoryItemToWrite()
            {
                Id = itemToUpdate.Id,
                Description = itemToUpdate.Description,
                ItemNumber = updatedItemNumber,
                ItemType = itemToUpdate.ItemType,
                Inspection = InventoryItemInspectionHelper.ConvertToWriteDto(itemToUpdate.Inspection),
                Labor = InventoryItemHelper.ConvertToWriteDto(itemToUpdate.Labor),
                Manufacturer = ManufacturerHelper.ConvertToWriteDto(itemToUpdate.Manufacturer),
                Package = InventoryItemPackageHelper.ConvertToWriteDto(itemToUpdate.Package),
                Part = InventoryItemPartHelper.ConvertToWriteDto(itemToUpdate.Part),
                ProductCode = ProductCodeHelper.ConvertToWriteDto(itemToUpdate.ProductCode),
                Tire = InventoryItemTireHelper.ConvertToWriteDto(itemToUpdate.Tire),
                Warranty = InventoryItemWarrantyHelper.ConvertToWriteDto(itemToUpdate.Warranty)
            };

            var response = await HttpClient.PutAsync($"{Route}/{itemToUpdate.Id}", JsonContent.Create(updatedInventoryItem));

            response.EnsureSuccessStatusCode();
            var itemFromEndpoint = await HttpClient.GetFromJsonAsync<InventoryItemToRead>($"{Route}/{itemToUpdate.Id}");
            itemFromEndpoint.Should().NotBeNull();
            itemFromEndpoint.ItemNumber.Should().NotBe(originalItemNumber);
            itemFromEndpoint.ItemNumber.Should().Be(updatedItemNumber);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_an_InventoryItem_Part()
        {
            var itemToUpdate = DbContext.InventoryItems.First();
            var originalId = itemToUpdate.Part.Id;
            var originalList = itemToUpdate.Part.List;
            var originalCost = itemToUpdate.Part.Cost;
            var originalCore = itemToUpdate.Part.Core;
            var originalRetail = itemToUpdate.Part.Retail;
            var originalLineCode = itemToUpdate.Part.LineCode;
            var originalSubLineCode = itemToUpdate.Part.SubLineCode;
            var originalFractional = itemToUpdate.Part.Fractional;
            var originalExciseFees = CreateOriginalExciseFees(itemToUpdate.Part.ExciseFees);
            var updatedAmount = Math.Round(Faker.Random.Double(0, 99), 2);
            var updatedTechAmount = TechAmount.Create(ItemLaborType.Flat, updatedAmount, SkillLevel.A).Value;
            var updatedLineCode = Faker.Random.AlphaNumeric(10).ToUpper();
            var updatedSubLineCode = Faker.Random.AlphaNumeric(10).ToLower();
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

            var response = await HttpClient.PutAsync($"{Route}/{itemToUpdate.Id}", JsonContent.Create(updatedInventoryItem));

            response.EnsureSuccessStatusCode();
            var itemFromEndpoint = await HttpClient.GetFromJsonAsync<InventoryItemToRead>($"{Route}/{itemToUpdate.Id}");
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

        private async Task<string> PostInventoryItem(InventoryItemToWrite inventoryItem)
        {
            var json = JsonSerializer.Serialize(inventoryItem, JsonSerializerHelper.DefaultSerializerOptions);
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

        private InventoryItemToWrite CreateInventoryItemToPost() => new()
        {
            Description = new Faker().Commerce.ProductDescription().Truncate(255),
            Manufacturer = ManufacturerHelper.ConvertToReadDto(DbContext.Manufacturers.First()),
            ItemNumber = new Faker().Random.AlphaNumeric(10).ToUpper(),
            ItemType = InventoryItemType.Part,
            ProductCode = ProductCodeHelper.ConvertToReadDto(DbContext.ProductCodes.First()),
            Part = InventoryItemPartHelper.ConvertToWriteDto(DbContext.InventoryItemParts.First())
        };

        public override void SeedData()
        {
            var count = 2;
            var collectionCount = 3;
            var saleCodeShopSuppliesCount = 40;
            var generateId = false;
            var saleCodeShopSupplies = new SaleCodeShopSuppliesFaker(generateId).Generate(saleCodeShopSuppliesCount);
            DataSeeder.Save(saleCodeShopSupplies);
            var selectedShopSupply = saleCodeShopSupplies[new Random().Next(saleCodeShopSupplies.Count)];
            var saleCodes = SaleCodeMaker.GenerateSaleCodes(selectedShopSupply);
            DataSeeder.Save(saleCodes);

            var manufacturers = new ManufacturerFaker(false).Generate(count);
            DataSeeder.Save(manufacturers);

            var saleCode = DbContext.SaleCodes.First();
            var manufacturer = DbContext.Manufacturers.First();
            var productCodes = new ProductCodeFaker(generateId, saleCodeFromCaller: saleCode, manufacturerFromCaller: manufacturer).Generate(count);
            DataSeeder.Save(productCodes);

            var inventoryItems = new InventoryItemFaker(generateId, collectionCount, manufacturer).Generate(count);

            DataSeeder.Save(inventoryItems);
        }
    }
}
