using Bogus;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Tests.Helpers;
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
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    // TODO: Mock httpClient or test the endpoints directly for CI/CD
    [Collection("Integration")]
    public class VendorInvoicesControllerShould : IClassFixture<IntegrationTestWebApplicationFactory>, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IDataSeeder dataSeeder;
        private readonly ApplicationDbContext dbContext;
        private const string route = "vendorinvoices";

        public VendorInvoicesControllerShould(IntegrationTestWebApplicationFactory factory)
        {
            httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                // TODO: can't really do this in CI/CD
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
            var response = await httpClient.GetAsync($"{route}/0");

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
            var vendor = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{route}/{vendorFromDatabase.Id}");

            vendor.Should().BeOfType<VendorInvoiceToRead>();
        }

        [Fact]
        public async Task Get_Valid_Id_Returns_Invoice()
        {
            var invoiceFromDatabase = dbContext.VendorInvoices.First();

            int lineItemsCount = invoiceFromDatabase.LineItems.Count;
            int paymentsCount = invoiceFromDatabase.Payments.Count;
            int taxesCount = invoiceFromDatabase.Taxes.Count;

            var invoiceFromEndpoint = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{route}/{invoiceFromDatabase.Id}");

            invoiceFromEndpoint.Should().BeOfType<VendorInvoiceToRead>();
            invoiceFromEndpoint.LineItems.Count.Should().Be(lineItemsCount);
            invoiceFromEndpoint.Payments.Count.Should().Be(paymentsCount);
            invoiceFromEndpoint.Taxes.Count.Should().Be(taxesCount);
        }

        [Fact]
        public async Task Add_an_Invoice()
        {
            var invoice = CreateInvoiceToPost();

            var result = await PostInvoice(invoice);

            var id = JsonSerializerHelper.GetIdFromString(result);
            var invoiceFromEndpoint = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{route}/{id}");
            invoiceFromEndpoint.Should().BeOfType<VendorInvoiceToRead>();
            invoiceFromEndpoint.Status.Should().Be(VendorInvoiceStatus.Open);
            invoiceFromEndpoint.DatePosted.Should().Be(null);
            invoiceFromEndpoint.LineItems.Count.Should().Be(0);
            invoiceFromEndpoint.Payments.Count.Should().Be(0);
            invoiceFromEndpoint.Taxes.Count.Should().Be(0);
        }

        [Fact]
        public async Task Update_an_Invoice()
        {
            var invoiceToUpdate = dbContext.VendorInvoices.First();
            var updatedInvoiceNumber = "INV-12345";
            var updatedInvoice = new VendorInvoiceToWrite()
            {
                DocumentType = invoiceToUpdate.DocumentType,
                Date = invoiceToUpdate.Date,
                Status = invoiceToUpdate.Status,
                InvoiceNumber = updatedInvoiceNumber,
                Total = invoiceToUpdate.Total,
                Vendor = VendorHelper.ConvertToReadDto(invoiceToUpdate.Vendor)
            };

            var response = await httpClient.PutAsync($"{route}/{invoiceToUpdate.Id}", JsonContent.Create(updatedInvoice));

            response.EnsureSuccessStatusCode();
            var invoiceFromEndpoint = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{route}/{invoiceToUpdate.Id}");
            invoiceFromEndpoint.Should().NotBeNull();
            invoiceFromEndpoint.InvoiceNumber.Should().Be(updatedInvoiceNumber);
        }

        //[Fact]
        // public async Task Update_an_Invoice_LineItems() {}

        //[Fact]
        // public async Task Update_an_Invoice_Payments() {}

        //[Fact]
        // public async Task Update_an_Invoice_Taxes() {}


        [Fact]
        public async Task Delete_an_Invoice()
        {
            var invoiceToDelete = dbContext.VendorInvoices.First();
            invoiceToDelete.Should().NotBeNull();

            var response = await httpClient.DeleteAsync($"{route}/{invoiceToDelete.Id}");

            response.EnsureSuccessStatusCode();
            var deletedInvoiceFromDatabase = dbContext.VendorInvoices.FirstOrDefault(invoice => invoice.Id == invoiceToDelete.Id);
            deletedInvoiceFromDatabase.Should().BeNull();
        }

        private VendorInvoiceToWrite CreateInvoiceToPost() => new()
        {
            DocumentType = VendorInvoiceDocumentType.Invoice,
            Status = VendorInvoiceStatus.Open,
            InvoiceNumber = VendorInvoiceFaker.GenerateInvoiceNumber(new Faker()),
            Total = 213.61,
            Vendor = VendorHelper.ConvertToReadDto(dbContext.Vendors.First())
        };

        private async Task<string> PostInvoice(VendorInvoiceToWrite invoice)
        {
            {
                var json = JsonSerializer.Serialize(invoice, JsonSerializerHelper.DefaultSerializerOptions);
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
            var invoices = new VendorInvoiceFaker().Generate(2);

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
            dbContext.VendorInvoicePaymentMethods
                .ToList()
                .ForEach(paymentMethod => paymentMethod
                .RemoveReconcilingVendor());

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);

            dbContext.Vendors
                .ToList()
                .ForEach(vendor => vendor
                .ClearDefaultPaymentMethod());

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);

            dbContext.VendorInvoicePayments.RemoveRange(dbContext.VendorInvoicePayments.ToList());
            dbContext.VendorInvoiceTaxes.RemoveRange(dbContext.VendorInvoiceTaxes.ToList());
            dbContext.VendorInvoiceLineItems.RemoveRange(dbContext.VendorInvoiceLineItems.ToList());
            dbContext.ExciseFees.RemoveRange(dbContext.ExciseFees.ToList());
            dbContext.SalesTaxes.RemoveRange(dbContext.SalesTaxes.ToList());
            dbContext.Manufacturers.RemoveRange(dbContext.Manufacturers.ToList());
            dbContext.SaleCodeShopSupplies.RemoveRange(dbContext.SaleCodeShopSupplies.ToList());
            dbContext.SaleCodes.RemoveRange(dbContext.SaleCodes.ToList());
            dbContext.VendorInvoicePaymentMethods.RemoveRange(dbContext.VendorInvoicePaymentMethods.ToList());
            dbContext.VendorInvoices.RemoveRange(dbContext.VendorInvoices.ToList());
            dbContext.Vendors.RemoveRange(dbContext.Vendors.ToList());

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }
    }
}
