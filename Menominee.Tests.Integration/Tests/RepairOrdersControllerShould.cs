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
    public class RepairOrdersControllerShould : IntegrationTestBase
    {
        private const string Route = "repairorders";
        private readonly long RepairOrderNumber = 36454531;
        private readonly long InvoiceNumber = 346181485;
        private readonly Faker Faker = new();
        public RepairOrdersControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_Returns_Expected_Response()
        {
            var repairOrderFromDatabase = DbContext.RepairOrders.First();

            var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{repairOrderFromDatabase.Id}");

            repairOrderFromEndpoint.Should().BeOfType<RepairOrderToRead>();
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
            var repairOrderFromDatabase = DbContext.RepairOrders.First();

            var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{repairOrderFromDatabase.Id}");

            repairOrderFromEndpoint.Should().BeOfType<RepairOrderToRead>();
            repairOrderFromEndpoint.Id.Should().Be(repairOrderFromDatabase.Id);
        }

        [Fact]
        public async Task Add_a_RepairOrder()
        {
            var collectionCount = 2;
            var repairOrderToPost = CreateRepairOrderToPost();
            var saleCode = DbContext.SaleCodes.First();
            var productCode = DbContext.ProductCodes.First();
            var manufacturer = DbContext.Manufacturers.First();
            var employees = DbContext.Employees.ToList();

            var services = new RepairOrderServiceFaker(
                generateId: false,
                saleCodeFromCaller: saleCode,
                lineItemsCount: collectionCount,
                techniciansCount: collectionCount,
                taxesCount: collectionCount,
                productCodeFromCaller: productCode,
                manufacturerFromCaller: manufacturer,
                employees: employees)
                .Generate(collectionCount);

            var payments = new RepairOrderPaymentFaker(false).Generate(collectionCount); // No child entities
            var paymentsToPost = PaymentHelper.ConvertToWriteDtos(payments);
            var statuses = new RepairOrderStatusFaker(false).Generate(collectionCount);
            var statusesToPost = StatusHelper.ConvertToWriteDtos(statuses);
            var taxes = new RepairOrderTaxFaker(false).Generate(collectionCount);
            var taxesToPost = RepairOrderTaxHelper.ConvertToWriteDtos(taxes);
            // TODO: Fix mapping issue a few levels deep: LineItemHelper.CovertToWriteDtos
            // var servicesToPost = ServiceHelper.ConvertToWriteDtos(services);
            repairOrderToPost.Payments = paymentsToPost;
            //repairOrderToPost.Services = servicesToPost;
            repairOrderToPost.Statuses = statusesToPost;
            repairOrderToPost.Taxes = taxesToPost;

            var repairOrderResult = await PostRepairOrder(repairOrderToPost);
            var id = JsonSerializerHelper.GetIdFromString(repairOrderResult);
            var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{id}");

            repairOrderFromEndpoint.Should().BeOfType<RepairOrderToRead>();
            repairOrderFromEndpoint.DateCreated.Value.Date.Should().Be(DateTime.Today);
            repairOrderFromEndpoint.DateModified.Value.Date.Should().Be(DateTime.Today);
            repairOrderFromEndpoint.Taxes.Should().HaveCount(collectionCount);
            repairOrderFromEndpoint.Payments.Should().HaveCount(collectionCount);
            repairOrderFromEndpoint.Statuses.Should().HaveCount(collectionCount);
            //repairOrderFromEndpoint.Services.Should().HaveCount(collectionCount);

            foreach (var service in repairOrderToPost.Services)
            {
                service.Taxes.Should().HaveCount(collectionCount);
                service.Techs.Should().HaveCount(collectionCount);
                //service.LineItems.Should().HaveCount(collectionCount);
            }

        }

        [Fact]
        public async Task Update_RepairOrder()
        {
            // Create a Repair Order
            var repairOrderToPost = CreateRepairOrderToPost();
            var collectionCount = 2;
            // Payments
            var updatedPaymentAmount = 11.11;
            var originalPaymentMethod = PaymentMethod.Cash;
            var updatedPaymentMethod = PaymentMethod.Card;
            var payments = new RepairOrderPaymentFaker(false).Generate(collectionCount); // No child entities
            payments.ForEach(payment => payment.SetPaymentMethod(originalPaymentMethod));
            var paymentsToPost = PaymentHelper.ConvertToWriteDtos(payments);
            repairOrderToPost.Payments = paymentsToPost;
            //Taxes
            var originalTaxRate = .7;
            var originalTaxAmount = 7;
            var updatedTaxRate = .9;
            var originalLaborTax = LaborTax.Create(originalTaxRate, originalTaxAmount).Value;
            var originalPartTax = PartTax.Create(originalTaxRate, originalTaxAmount).Value;
            var updatedLaborTax = LaborTax.Create(updatedTaxRate, originalTaxAmount + updatedPaymentAmount).Value;
            var updatedPartTax = PartTax.Create(updatedTaxRate, originalTaxAmount + updatedPaymentAmount).Value;
            var taxes = new RepairOrderTaxFaker(false).Generate(collectionCount);
            taxes.ForEach(tax =>
            {
                tax.SetLaborTax(originalLaborTax);
                tax.SetPartTax(originalPartTax);
            });
            var taxesToPost = RepairOrderTaxHelper.ConvertToWriteDtos(taxes);
            repairOrderToPost.Taxes = taxesToPost;
            // Statuses
            var satuses = new RepairOrderStatusFaker(false).Generate(collectionCount);
            var originalStatus = Status.New;
            var originalDescription = satuses[0].Description;
            var updatedStatus = Status.Approved;
            var updatedDescription = "Updated Description";
            satuses.ForEach(status =>
            {
                status.SetDescription(originalDescription);
                status.SetStatus(originalStatus);
            });
            var satusesToPost = StatusHelper.ConvertToWriteDtos(satuses);
            repairOrderToPost.Statuses = satusesToPost;
            // POST the RepairOrder to the controller
            var repairOrderPostResult = await PostRepairOrder(repairOrderToPost);
            var id = JsonSerializerHelper.GetIdFromString(repairOrderPostResult);
            // GET the posted RepairOrder from the controller
            var repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{id}");
            var originalPaymentsToPost = repairOrderFromEndpoint.Payments;
            var originalTaxesToPost = repairOrderFromEndpoint.Taxes;
            var originalStatusesToPost = repairOrderFromEndpoint.Statuses;
            var updatedRepairOrderNumber = 1234567L;
            var repairOrderToPut = new RepairOrderToWrite()
            {
                Id = id,
                AccountingDate = repairOrderFromEndpoint.AccountingDate,
                Customer = repairOrderFromEndpoint.Customer,
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
                Vehicle = repairOrderFromEndpoint.Vehicle,
                Payments = PaymentHelper.ConvertReadToWriteDtos(repairOrderFromEndpoint.Payments),
                Taxes = RepairOrderTaxHelper.CovertReadToWriteDtos(repairOrderFromEndpoint.Taxes),
                Statuses = StatusHelper.CovertReadToWriteDtos(repairOrderFromEndpoint.Statuses)
            };
            // Update the RepairOrder from the controller
            foreach (var payment in repairOrderToPut.Payments)
            {
                payment.Amount = updatedPaymentAmount;
                payment.PaymentMethod = updatedPaymentMethod;
                var originalPayment = originalPaymentsToPost.Find(oPayment => oPayment.Id == payment.Id);
                payment.Amount.Should().Be(updatedPaymentAmount);
                payment.PaymentMethod.Should().Be(updatedPaymentMethod);
            }

            foreach (var tax in repairOrderToPut.Taxes)
            {
                tax.LaborTax = new()
                {
                    Amount = updatedLaborTax.Amount,
                    Rate = updatedLaborTax.Rate
                };
                tax.PartTax = new()
                {
                    Amount = updatedPartTax.Amount,
                    Rate = updatedPartTax.Rate
                };
                var originalTax = originalTaxesToPost.Find(oTax => oTax.Id == tax.Id);
                tax.LaborTax.Should().NotBeEquivalentTo(originalTax.LaborTax);
                tax.PartTax.Should().NotBeEquivalentTo(originalTax.PartTax);
            }

            foreach (var status in repairOrderToPut.Statuses)
            {
                status.Status = updatedStatus;
                status.Description = updatedDescription;
                var oStatus = originalStatusesToPost.Find(oStatus => oStatus.Id == status.Id);
                oStatus.Status.Should().Be(originalStatus);
                status.Status.Should().Be(updatedStatus);
                status.Description.Should().Be(updatedDescription);
            }

            updatedRepairOrderNumber.Should().NotBe(repairOrderFromEndpoint.RepairOrderNumber);

            // Send updates to controller
            var response = await HttpClient.PutAsync($"{Route}/{repairOrderToPut.Id}", JsonContent.Create(repairOrderToPut));
            response.EnsureSuccessStatusCode();
            // Get updated RepairOrder from controller
            repairOrderFromEndpoint = await HttpClient.GetFromJsonAsync<RepairOrderToRead>($"{Route}/{repairOrderToPut.Id}");

            // ASSERT :)
            repairOrderFromEndpoint.Should().NotBeNull();
            repairOrderFromEndpoint.RepairOrderNumber.Should().Be(updatedRepairOrderNumber);
            repairOrderFromEndpoint.Payments.Count.Should().Be(repairOrderToPost.Payments.Count);
            repairOrderFromEndpoint.Taxes.Count.Should().Be(repairOrderToPost.Taxes.Count);
            // Payments
            foreach (var payment in repairOrderFromEndpoint.Payments)
            {
                var originalPayment = originalPaymentsToPost.Find(oPayment => oPayment.Id == payment.Id);
                payment.Amount.Should().NotBe(originalPayment.Amount);
                payment.PaymentMethod.Should().NotBe(originalPayment.PaymentMethod);
                payment.Amount.Should().Be(updatedPaymentAmount);
                payment.PaymentMethod.Should().Be(updatedPaymentMethod);
            }
            //Taxes
            foreach (var tax in repairOrderFromEndpoint.Taxes)
            {
                var originalTax = originalTaxesToPost.Find(oTax => oTax.Id == tax.Id);
                tax.LaborTax.Amount.Should().NotBe(originalTax.LaborTax.Amount);
                tax.LaborTax.Rate.Should().NotBe(originalTax.LaborTax.Rate);
                tax.PartTax.Amount.Should().NotBe(originalTax.PartTax.Amount);
                tax.PartTax.Rate.Should().NotBe(originalTax.PartTax.Rate);
                tax.LaborTax.Amount.Should().Be(updatedLaborTax.Amount);
                tax.LaborTax.Rate.Should().Be(updatedLaborTax.Rate);
                tax.PartTax.Amount.Should().Be(updatedPartTax.Amount);
                tax.PartTax.Rate.Should().Be(updatedPartTax.Rate);
            }
            // Statuses
            foreach (var status in repairOrderToPut.Statuses)
            {
                var oStatus = originalStatusesToPost.Find(oStatus => oStatus.Id == status.Id);
                status.Status.Should().NotBe(oStatus.Status);
                status.Description.Should().NotBe(oStatus.Description);
                status.Status.Should().Be(updatedStatus);
                status.Description.Should().Be(updatedDescription);
            }
        }

        private RepairOrderToWrite CreateRepairOrderToPost()
        {
            var accountingDate = Faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
            var customer = DbContext.Customers.FirstOrDefault();
            var vehicle = customer.Vehicles.Count > 0 ? customer.Vehicles[0] : DbContext.Vehicles.FirstOrDefault();

            var customerToReadDto = CustomerHelper.ConvertToReadDto(customer);
            var vehicleToReadDto = VehicleHelper.ConvertToReadDto(vehicle);

            var payments = new List<RepairOrderPaymentToWrite>();
            var services = new List<RepairOrderServiceToWrite>();
            var statuses = new List<RepairOrderStatusToWrite>();
            var taxes = new List<RepairOrderTaxToWrite>();

            var repairOrderToWrite = new RepairOrderToWrite
            {
                AccountingDate = accountingDate,
                Customer = customerToReadDto,
                DateCreated = DateTime.Today,
                DateModified = DateTime.Today,
                Vehicle = vehicleToReadDto,
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
                    return await response.Content.ReadAsStringAsync();

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
            var generateId = false;
            var faker = new Faker();

            var persons = new PersonFaker(generateId: false, includeAddress: true)
                .Generate(count);
            var businesses = new BusinessFaker(generateId: false, includeAddress: true)
                .Generate(count);

            DataSeeder.Save(persons);
            DataSeeder.Save(businesses);

            var customers = new List<Customer>();

            for (var i = 0; i < count; i++)
            {
                var customerType = (i % 2 == 0) ? CustomerType.Retail : CustomerType.Business;

                if (customerType == CustomerType.Business)
                {
                    var customer = Customer.Create(
                        businesses[i],
                        customerType,
                        code: string.Empty)
                        .Value;

                    customers.Add(customer);
                }

                if (customerType == CustomerType.Retail)
                {
                    var customer = Customer.Create(
                        persons[i],
                        customerType,
                        code: string.Empty)
                        .Value;

                    customers.Add(customer);
                }
            }

            DataSeeder.Save(customers);

            var vehicles = new VehicleFaker(false).Generate(count);
            DataSeeder.Save(vehicles);

            for (var i = 0; i < count; i++)
            {
                customers[i].AddVehicle(vehicles[i]);
            }

            DataSeeder.Save(customers);

            var accountingDate = faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
            var repairOrderNumbers = new List<long>();
            var lastInvoiceNumber = faker.Random.Long(1000, 100000);

            var repairOrders = customers.Select(customer =>
                RepairOrder.Create(
                    customer,
                    customer.Vehicles[0],
                    accountingDate,
                    repairOrderNumbers,
                    lastInvoiceNumber).Value
                ).ToList();

            DataSeeder.Save(repairOrders);

            var saleCodes = SaleCodeMaker.GenerateSaleCodes();
            DataSeeder.Save(saleCodes);

            var employees = new EmployeeFaker(false, count).Generate(count);
            DataSeeder.Save(employees);

            var manufacturers = new ManufacturerFaker(false).Generate(count);
            DataSeeder.Save(manufacturers);

            var saleCode = DbContext.SaleCodes.First();
            var manufacturer = DbContext.Manufacturers.First();
            var productCodes = new ProductCodeFaker(generateId, saleCodeFromCaller: saleCode, manufacturerFromCaller: manufacturer).Generate(count);
            DataSeeder.Save(productCodes);

            //var productCode = DbContext.ProductCodes.First();
            //var items = new RepairOrderItemFaker(generateId, saleCodeFromCaller: saleCode, manufacturerFromCaller: manufacturer, productCodeFromCaller: productCode).Generate(count);
            //DataSeeder.Save(items);
        }
    }
}
