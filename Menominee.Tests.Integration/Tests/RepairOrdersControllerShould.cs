using Bogus;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.Payments;
using Menominee.Shared.Models.RepairOrders.Services;
using Menominee.Shared.Models.RepairOrders.Statuses;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

public class RepairOrdersControllerShould : IntegrationTestBase
{
    private const string Route = "repairorders";
    private readonly long RepairOrderNumber = 36454531;
    private readonly long InvoiceNumber = 346181485;
    private readonly Faker Faker = new();
    private readonly string code = "Code";
    public RepairOrdersControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Route_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync("invalid-route");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}/0a");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Id_Returns_Invoice()
    {
        var repairOrderToPost = CreateRepairOrderToPost();
        var repairOrderResult = await PostRepairOrder(repairOrderToPost);
        var id = JsonSerializerHelper.GetIdFromString(repairOrderResult);
        var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{id}");

        repairOrderFromEndpoint.Should().BeOfType<RepairOrderToRead>();
    }

    [Fact]
    public async Task Add_a_RepairOrder()
    {
        var repairOrderToPost = CreateRepairOrderToPost();

        var repairOrderResult = await PostRepairOrder(repairOrderToPost);

        var id = JsonSerializerHelper.GetIdFromString(repairOrderResult);
        var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{id}");
        repairOrderFromEndpoint.Should().BeOfType<RepairOrderToRead>();
        repairOrderFromEndpoint.DateCreated.Value.Date.Should().Be(DateTime.Today);
        repairOrderFromEndpoint.DateModified.Value.Date.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task Update_a_RepairOrder()
    {
        var repairOrderToPost = CreateRepairOrderToPost();
        var repairOrderPostResult = await PostRepairOrder(repairOrderToPost);
        var id = JsonSerializerHelper.GetIdFromString(repairOrderPostResult);
        var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{id}");
        var updatedRepairOrderNumber = 1234567L;
        var repairOrderToPut = new RepairOrderToWrite()
        {
            Id = id,
            AccountingDate = repairOrderFromEndpoint.AccountingDate,
            Customer = CustomerHelper.ConvertReadToWriteDto(repairOrderFromEndpoint.Customer),
            DateCreated = repairOrderFromEndpoint.DateCreated,
            DateModified = repairOrderFromEndpoint.DateModified,
            DiscountTotal = repairOrderFromEndpoint.DiscountTotal,
            HazMatTotal = repairOrderFromEndpoint.HazMatTotal,
            InvoiceNumber = repairOrderFromEndpoint.InvoiceNumber,
            LaborTotal = repairOrderFromEndpoint.LaborTotal,
            PartsTotal = repairOrderFromEndpoint.PartsTotal,
            RepairOrderNumber = updatedRepairOrderNumber,
            ShopSuppliesTotal = repairOrderFromEndpoint.ShopSuppliesTotal,
            TaxTotal = repairOrderFromEndpoint.TaxTotal,
            Total = repairOrderFromEndpoint.Total,
            Vehicle = VehicleHelper.ConvertReadToWriteDto(repairOrderFromEndpoint.Vehicle)
        };
        updatedRepairOrderNumber.Should().NotBe(repairOrderFromEndpoint.RepairOrderNumber);

        var response = await HttpClient.PutAsync($"{Route}/{repairOrderToPut.Id}", JsonContent.Create(repairOrderToPut));
        response.EnsureSuccessStatusCode();
        repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{repairOrderToPut.Id}");

        repairOrderFromEndpoint.Should().NotBeNull();
        repairOrderFromEndpoint.RepairOrderNumber.Should().Be(updatedRepairOrderNumber);
    }

    private RepairOrderToWrite CreateRepairOrderToPost()
    {
        var accountingDate = Faker.Date
            .Between(
                DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays),
                DateTime.Today).AddYears(-1);

        var person = new PersonFaker(generateId: false, includeAddress: true, includeDriversLicense: true)
            .Generate();

        var customer = Customer.Create(
            person,
            CustomerType.Retail,
            code: code)
            .Value;

        var vehicle = new VehicleFaker(false).Generate();
        customer.AddVehicle(vehicle);

        var customerToReadDto = CustomerHelper.ConvertToReadDto(customer);

        var payments = new List<RepairOrderPaymentToWrite>();
        var services = new List<RepairOrderServiceToWrite>();
        var statuses = new List<RepairOrderStatusToWrite>();
        var taxes = new List<RepairOrderTaxToWrite>();

        var repairOrderToWrite = new RepairOrderToWrite
        {
            AccountingDate = accountingDate,
            Customer = CustomerHelper.ConvertReadToWriteDto(customerToReadDto),
            DateCreated = DateTime.Today,
            DateModified = DateTime.Today,
            Vehicle = VehicleHelper.ConvertReadToWriteDto(customerToReadDto.Vehicles[0]),
            InvoiceNumber = InvoiceNumber,
            RepairOrderNumber = RepairOrderNumber,
            Payments = payments,
            Services = services,
            Statuses = statuses,
            Taxes = taxes
        };

        return repairOrderToWrite;
    }

    private async Task<string> PostRepairOrder(RepairOrderToWrite repairOrder)
    {
        {
            var json = JsonSerializer.Serialize(repairOrder, JsonSerializerHelper.DefaultSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.PostAsync(Route, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var errorContent = await response.Content.ReadAsStringAsync();

            var (success, apiError) = JsonSerializerHelper.DeserializeApiError(errorContent);

            return success
                ? $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}"
                : throw new JsonException("Failed to deserialize ApiError");
        }
    }

    public override void SeedData()
    {
        var count = 3;
        var persons = new PersonFaker(generateId: false, includeAddress: true)
            .Generate(count);
        var businesses = new BusinessFaker(generateId: false, includeAddress: true)
            .Generate(count);

        DataSeeder.Save(persons);
        DataSeeder.Save(businesses);

        //var customers = new List<Customer>();
        //for (var i = 0; i < count; i++)
        //{
        //    var entityType = (i % 2 == 0) ? EntityType.Person : EntityType.Business;

        //    if (entityType == EntityType.Business)
        //    {
        //        var customer = Customer.Create(
        //            businesses[i],
        //            CustomerType.Retail,
        //            code: code)
        //            .Value;

        //        customers.Add(customer);
        //    }

        //    if (entityType == EntityType.Person)
        //    {
        //        var customer = Customer.Create(
        //            persons[i],
        //            CustomerType.Retail,
        //            code: code)
        //            .Value;

        //        customers.Add(customer);
        //    }
        //}
        //DataSeeder.Save(customers);

        //var vehicles = new VehicleFaker(false).Generate(count);
        //DataSeeder.Save(vehicles);

        //for (var i = 0; i < count; i++)
        //{
        //    customers[i].AddVehicle(vehicles[i]);
        //}

        //DataSeeder.Save(customers);
    }
}
