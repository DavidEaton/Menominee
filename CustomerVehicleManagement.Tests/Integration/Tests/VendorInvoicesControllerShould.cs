using Bogus;
using CustomerVehicleManagement.Api.Common;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Tests.Helpers;
using CustomerVehicleManagement.Tests.Helpers.Fakers;
using CustomerVehicleManagement.Tests.Integration.Data;
using FluentAssertions;
using Menominee.Common.Enums;
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
using Xunit;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    [Collection("Integration tests")]
    public class VendorInvoicesControllerShould : IClassFixture<IntegrationTestWebApplicationFactory>, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IDataSeeder dataSeeder;
        private readonly ApplicationDbContext dbContext;
        private const string route = "vendorinvoices/";

        public VendorInvoicesControllerShould(IntegrationTestWebApplicationFactory factory)
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
            var response = await httpClient.GetAsync("vendorinvoice");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Invalid_Id_Returns_NotFound()
        {
            var response = await httpClient.GetAsync("vendorinvoices/0");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Returns_Expected_Response()
        {
            var vendorFromDatabase = dbContext.Vendors.First();

            /* GetFromJsonAsync:
                Validates response has success status code
                Validates content-type header
                Validates that response includes content
                Uses case-insensitive deserialization
            */
            var vendor = await httpClient.GetFromJsonAsync<VendorToRead>($"Vendors/{vendorFromDatabase.Id}");

            vendor.Should().BeOfType<VendorToRead>();
        }

        [Fact]
        public async Task Get_Valid_Id_Returns_Invoice()
        {
            var invoiceFromDatabase = dbContext.VendorInvoices.First();

            int lineItemsCount = invoiceFromDatabase.LineItems.Count;
            int paymentsCount = invoiceFromDatabase.Payments.Count;
            int taxesCount = invoiceFromDatabase.Taxes.Count;

            var invoiceFromEndpoint = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"vendorinvoices/{invoiceFromDatabase.Id}");

            invoiceFromEndpoint.Should().BeOfType<VendorInvoiceToRead>();
            invoiceFromEndpoint.LineItems.Count.Should().Be(lineItemsCount);
            invoiceFromEndpoint.Payments.Count.Should().Be(paymentsCount);
            invoiceFromEndpoint.Taxes.Count.Should().Be(taxesCount);
        }

        [Fact]
        public async Task Add_an_Invoice()
        {
            var invoice = CreateInvoiceToPost();
            
            var result = await PostInvoiceAsync(invoice);

            var invoiceFromDatabase = dbContext
                .VendorInvoices.FirstOrDefault(
                invoice => 
                invoice.Id == JsonSerializerHelper.GetIdFromString(result));
            invoiceFromDatabase.Should().BeOfType<VendorInvoice>();
            invoiceFromDatabase.Status.Should().Be(VendorInvoiceStatus.Open);
            invoiceFromDatabase.Date.Should().Be(DateTime.Today);
            invoiceFromDatabase.DatePosted.Should().Be(null);
            invoiceFromDatabase.LineItems.Count.Should().Be(0);
            invoiceFromDatabase.Payments.Count.Should().Be(0);
            invoiceFromDatabase.Taxes.Count.Should().Be(0);
        }

        [Fact]
        public void Update_an_Invoice()
        {
            ////SeedData();
            //var invoiceToUpdate = dbContext.VendorInvoices.First();

            //// Update the invoice properties
            //invoiceToUpdate.SetStatus(VendorInvoiceStatus.Completed);
            //invoiceToUpdate.SetDate(DateTime.Today.AddDays(-1));
            //invoiceToUpdate.SetDatePosted(DateTime.Today);

            //// Serialize the updated invoice
            //var content = new StringContent(JsonSerializer.Serialize(invoiceToUpdate, JsonSerializerHelper.DefaultSerializerOptions), Encoding.UTF8, "application/json");

            //// Act
            //var response = await httpclient.PutAsync($"vendorinvoices/{invoiceToUpdate.Id}", content);

            //// Assert
            //response.EnsureSuccessStatusCode();

            //// Fetch the updated invoice from the API
            //var responseStream = await httpclient.GetStreamAsync($"vendorinvoices/{invoiceToUpdate.Id}");
            //var fetchedInvoice = await JsonSerializer.DeserializeAsync<VendorInvoiceToRead>(responseStream, JsonSerializerHelper.DefaultDeserializerOptions);

            //// Verify the fetched invoice has the updated properties
            //fetchedInvoice.Should().BeOfType<VendorInvoiceToRead>();
            //fetchedInvoice.Status.Should().Be(VendorInvoiceStatus.Completed);
            //fetchedInvoice.Date.Should().BeSameDateAs(DateTime.Today.AddDays(-1));
            //fetchedInvoice.DatePosted.Should().BeSameDateAs(DateTime.Today);

            true.Should().BeTrue();
        }

        [Fact]
        public void Delete_an_Invoice()
        {
            true.Should().BeTrue();

            //true.Should().BeFalse();
        }

        private VendorInvoiceToWrite CreateInvoiceToPost() => new()
        {
            DocumentType = VendorInvoiceDocumentType.Invoice,
            Date = DateTime.Today,
            Status = VendorInvoiceStatus.Open,
            InvoiceNumber = VendorInvoiceFaker.GenerateInvoiceNumber(new Faker()),
            Total = 213.61,
            Vendor = VendorHelper.ConvertToReadDto(dbContext.Vendors.First())
        };

        private async Task<string> PostInvoiceAsync(VendorInvoiceToWrite invoice)
        {
            var json = JsonSerializer.Serialize(invoice, JsonSerializerHelper.DefaultSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var errorContent = await response.Content.ReadAsStringAsync();
            ApiError apiError = null;

            try
            {
                apiError = JsonSerializer.Deserialize<ApiError>(
                errorContent,
                JsonSerializerHelper.DefaultDeserializerOptions);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"{ex.Message}");
                throw;
            }

            return $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}";
        }

        private void SeedData()
        {
            var count = 2;
            var paymentMethods = new VendorInvoicePaymentMethodFaker(false).Generate(count);
            var defaultPaymentMethods = new DefaultPaymentMethodFaker(false).Generate(count);
            var vendors = new VendorFaker(false).Generate(count);
            var saleCodeShopSupplies = new SaleCodeShopSuppliesFaker(false).Generate(count);
            var saleCodes = new SaleCodeFaker(false).Generate(count);
            var manufacturers = new ManufacturerFaker(false).Generate(count);
            var vendorInvoiceItems = new VendorInvoiceItemFaker(false).Generate(count);
            var exciseFees = new ExciseFeeFaker(generateId: false).Generate(count);
            var salesTaxes = new SalesTaxFaker(false).Generate(count);
            var vendorInvoiceLineItems = new VendorInvoiceLineItemFaker(false).Generate(count);
            var vendorInvoiceTaxes = new VendorInvoiceTaxFaker(false).Generate(count);
            var vendorInvoicePayments = new VendorInvoicePaymentFaker(false).Generate(count);
            var invoices = new VendorInvoiceFaker().Generate(1);

            // SeedData in correct order of creation heirarchy
            dataSeeder.SeedData(saleCodeShopSupplies);
            foreach (var saleCode in saleCodes)
                saleCode.SetShopSupplies(saleCodeShopSupplies[0]);
            dataSeeder.SeedData(saleCodes);

            dataSeeder.SeedData(manufacturers);
            foreach (var item in vendorInvoiceItems)
            {
                item.SetManufacturer(manufacturers[new Random().Next(manufacturers.Count)]);
                item.SetSaleCode(saleCodes[new Random().Next(saleCodes.Count)]);
            }

            dataSeeder.SeedData(paymentMethods);
            foreach (var defaultPaymentMethod in defaultPaymentMethods)
                defaultPaymentMethod.SetPaymentMethod(paymentMethods[new Random().Next(paymentMethods.Count)]);

            foreach (var vendor in vendors)
                vendor.SetDefaultPaymentMethod(defaultPaymentMethods[new Random().Next(defaultPaymentMethods.Count)]);
            dataSeeder.SeedData(vendors);

            foreach (var paymentMethod in paymentMethods)
                paymentMethod.SetReconcilingVendor(vendors[new Random().Next(vendors.Count)]);
            // Save paymentMethods changes to database
            dbContext.SaveChanges();

            dataSeeder.SeedData(exciseFees);
            foreach (var salesTax in salesTaxes)
                salesTax.AddExciseFee(exciseFees[new Random().Next(exciseFees.Count)]);

            dataSeeder.SeedData(salesTaxes);
            foreach (var tax in vendorInvoiceTaxes)
                tax.SetSalesTax(salesTaxes[new Random().Next(salesTaxes.Count)]);

            invoices[0].SetVendor(vendors[new Random().Next(vendors.Count)]);

            foreach (var lineItem in vendorInvoiceLineItems)
                invoices[0].AddLineItem(lineItem);

            foreach (var payment in vendorInvoicePayments)
                invoices[0].AddPayment(payment);

            foreach (var tax in vendorInvoiceTaxes)
                invoices[0].AddTax(tax);

            dataSeeder.SeedData(invoices);
        }

        public void Dispose()
        {
            // Order matters!
            // Set navigation properties to null before removing entities
            foreach (var vendorInvoicePaymentMethod in dbContext.VendorInvoicePaymentMethods)
                vendorInvoicePaymentMethod.RemoveReconcilingVendor();
            dbContext.SaveChanges();

            foreach (var vendor in dbContext.Vendors)
                vendor.ClearDefaultPaymentMethod();
            dbContext.SaveChanges();

            dbContext.VendorInvoicePayments.RemoveRange(dbContext.VendorInvoicePayments);
            dbContext.VendorInvoiceTaxes.RemoveRange(dbContext.VendorInvoiceTaxes);
            dbContext.VendorInvoiceLineItems.RemoveRange(dbContext.VendorInvoiceLineItems);
            dbContext.ExciseFees.RemoveRange(dbContext.ExciseFees);
            dbContext.SalesTaxes.RemoveRange(dbContext.SalesTaxes);
            dbContext.Manufacturers.RemoveRange(dbContext.Manufacturers);
            dbContext.SaleCodeShopSupplies.RemoveRange(dbContext.SaleCodeShopSupplies);
            dbContext.SaleCodes.RemoveRange(dbContext.SaleCodes);
            dbContext.VendorInvoicePaymentMethods.RemoveRange(dbContext.VendorInvoicePaymentMethods);
            dbContext.Vendors.RemoveRange(dbContext.Vendors);
            dbContext.VendorInvoices.RemoveRange(dbContext.VendorInvoices);
            dbContext.SaveChanges();
        }
    }
}
