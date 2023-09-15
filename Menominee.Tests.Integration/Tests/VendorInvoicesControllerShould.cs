using Bogus;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Payables.Invoices;
using Menominee.Shared.Models.Payables.Vendors;
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

namespace Menominee.Tests.Integration.Tests
{
    // TODO: Mock httpClient or test the endpoints directly for CI/CD
    [Collection("Integration")]
    public class VendorInvoicesControllerShould : IntegrationTestBase
    {
        private const string Route = "vendorinvoices";
        public VendorInvoicesControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Invalid_Route_Returns_NotFound()
        {
            var response = await HttpClient.GetAsync("vendorinvoice");

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
            var vendorFromDatabase = DbContext.Vendors.First();

            /* GetFromJsonAsync:
                Validates response has success status code
                Validates content-type header
                Validates that response includes content
                Uses case-insensitive deserialization
            */
            var vendor = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{vendorFromDatabase.Id}");

            vendor.Should().BeOfType<VendorInvoiceToRead>();
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Get_Valid_Id_Returns_Invoice()
        {
            var invoiceFromDatabase = DbContext.VendorInvoices.First();

            int lineItemsCount = invoiceFromDatabase.LineItems.Count;
            int paymentsCount = invoiceFromDatabase.Payments.Count;
            int taxesCount = invoiceFromDatabase.Taxes.Count;

            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{invoiceFromDatabase.Id}");

            invoiceFromEndpoint.Should().BeOfType<VendorInvoiceToRead>();
            invoiceFromEndpoint.LineItems.Count.Should().Be(lineItemsCount);
            invoiceFromEndpoint.Payments.Count.Should().Be(paymentsCount);
            invoiceFromEndpoint.Taxes.Count.Should().Be(taxesCount);
            invoiceFromEndpoint.Id.Should().Be(invoiceFromDatabase.Id);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Add_an_Invoice()
        {
            var invoice = CreateInvoiceToPost();

            var result = await PostInvoice(invoice);

            var id = JsonSerializerHelper.GetIdFromString(result);
            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{id}");
            invoiceFromEndpoint.Should().BeOfType<VendorInvoiceToRead>();
            invoiceFromEndpoint.Status.Should().Be(VendorInvoiceStatus.Open);
            invoiceFromEndpoint.DatePosted.Should().Be(null);
            invoiceFromEndpoint.LineItems.Count.Should().Be(0);
            invoiceFromEndpoint.Payments.Count.Should().Be(0);
            invoiceFromEndpoint.Taxes.Count.Should().Be(0);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_an_Invoice()
        {
            var invoiceToUpdate = DbContext.VendorInvoices.First();
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

            var response = await HttpClient.PutAsync($"{Route}/{invoiceToUpdate.Id}", JsonContent.Create(updatedInvoice));

            response.EnsureSuccessStatusCode();
            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{invoiceToUpdate.Id}");
            invoiceFromEndpoint.Should().NotBeNull();
            invoiceFromEndpoint.InvoiceNumber.Should().Be(updatedInvoiceNumber);
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_Invoice_LineItems()
        {
            var invoiceToUpdate = DbContext.VendorInvoices.First();
            var poNumber = "Test PoNumber";
            var transactionDate = DateTime.Today;
            var core = 3548.19;
            var cost = 3461.87;
            var quantity = 4678;
            var item = new VendorInvoiceItemFaker(false).Generate();
            foreach (var line in invoiceToUpdate.LineItems)
            {
                line.PONumber.Should().NotBe(poNumber);
                line.SetPONumber(poNumber);

                line.TransactionDate.Should().NotBe(transactionDate);
                line.SetTransactionDate(transactionDate);

                line.Core.Should().NotBe(core);
                line.SetCore(core);

                line.Cost.Should().NotBe(cost);
                line.SetCost(cost);

                line.Quantity.Should().NotBe(quantity);
                line.SetQuantity(quantity);

                line.Item.Should().NotBe(item);
                line.SetItem(item);
            }
            var updatedInvoice = VendorInvoiceHelper.ConvertToWriteDto(invoiceToUpdate);

            var response = await HttpClient.PutAsync($"{Route}/{invoiceToUpdate.Id}", JsonContent.Create(updatedInvoice));

            response.EnsureSuccessStatusCode();
            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{invoiceToUpdate.Id}");
            invoiceFromEndpoint.Should().NotBeNull();
            foreach (var line in invoiceFromEndpoint.LineItems)
            {
                line.PONumber.Should().Be(poNumber);
                line.TransactionDate.Should().Be(transactionDate);
                line.Core.Should().Be(core);
                line.Cost.Should().Be(cost);
                line.Quantity.Should().Be(quantity);
                line.Item.Description.Should().Be(item.Description);
                line.Item.PartNumber.Should().Be(item.PartNumber);

                if (line.Item.Manufacturer is not null)
                    if (line.Item.Manufacturer.Id != 0)
                        line.Item.Manufacturer.Id.Should().Be(item.Manufacturer.Id);

                if (line.Item.SaleCode is not null)
                    if (line.Item.SaleCode.Id != 0)
                        line.Item.SaleCode.Id.Should().Be(item.SaleCode.Id);
            }
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_Invoice_Payments()
        {
            var invoiceToUpdate = DbContext.VendorInvoices.First();
            var amount = 3548.19;
            var paymentMethod = GetUnusedPaymentMethod(invoiceToUpdate);
            foreach (var payment in invoiceToUpdate.Payments)
            {
                payment.Amount.Should().NotBe(amount);
                payment.SetAmount(amount);

                payment.PaymentMethod.Id.Should().NotBe(paymentMethod.Id);
                payment.SetPaymentMethod(paymentMethod);
            }
            var updatedInvoice = VendorInvoiceHelper.ConvertToWriteDto(invoiceToUpdate);

            var response = await HttpClient.PutAsync($"{Route}/{invoiceToUpdate.Id}", JsonContent.Create(updatedInvoice));

            response.EnsureSuccessStatusCode();
            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{invoiceToUpdate.Id}");
            invoiceFromEndpoint.Should().NotBeNull();
            foreach (var payment in invoiceFromEndpoint.Payments)
            {
                payment.Amount.Should().Be(amount);
                payment.PaymentMethod.Id.Should().Be(paymentMethod.Id);
            }
        }
        private VendorInvoicePaymentMethod GetUnusedPaymentMethod(VendorInvoice invoiceToUpdate)
        {
            var paymentMethodsInUse = invoiceToUpdate.Payments
                .Select(payment => payment.PaymentMethod).ToList();

            return DbContext.VendorInvoicePaymentMethods
                .FirstOrDefault(paymentMethod => !paymentMethodsInUse.Contains(paymentMethod));
        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Update_Invoice_Taxes()
        {
            var invoiceToUpdate = DbContext.VendorInvoices.First();
            var amount = 3548.19;
            var salesTax = GetUnusedSalesTax(invoiceToUpdate);
            foreach (var tax in invoiceToUpdate.Taxes)
            {
                tax.Amount.Should().NotBe(amount);
                tax.SetAmount(amount);

                tax.SalesTax.Id.Should().NotBe(salesTax.Id);
                tax.SetSalesTax(salesTax);
            }
            var updatedInvoice = VendorInvoiceHelper.ConvertToWriteDto(invoiceToUpdate);

            var response = await HttpClient.PutAsync($"{Route}/{invoiceToUpdate.Id}", JsonContent.Create(updatedInvoice));

            response.EnsureSuccessStatusCode();
            var invoiceFromEndpoint = await HttpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{Route}/{invoiceToUpdate.Id}");
            invoiceFromEndpoint.Should().NotBeNull();
            foreach (var tax in invoiceFromEndpoint.Taxes)
            {
                tax.Amount.Should().Be(amount);
                tax.SalesTax.Id.Should().Be(salesTax.Id);
            }
        }

        private SalesTax GetUnusedSalesTax(VendorInvoice invoiceToUpdate)
        {
            var salesTaxInUse = invoiceToUpdate.Taxes
                .Select(tax => tax.SalesTax).ToList();

            return DbContext.SalesTaxes
                .FirstOrDefault(salesTax => !salesTaxInUse.Contains(salesTax));

        }

        [Fact(Skip = "Skipping until MEN-944 is resolved")]
        public async Task Delete_an_Invoice()
        {
            var invoiceToDelete = DbContext.VendorInvoices.First();
            invoiceToDelete.Should().NotBeNull();

            var response = await HttpClient.DeleteAsync($"{Route}/{invoiceToDelete.Id}");

            response.EnsureSuccessStatusCode();
            var deletedInvoiceFromDatabase = DbContext.VendorInvoices.FirstOrDefault(invoice => invoice.Id == invoiceToDelete.Id);
            deletedInvoiceFromDatabase.Should().BeNull();
        }

        private VendorInvoiceToWrite CreateInvoiceToPost() => new()
        {
            DocumentType = VendorInvoiceDocumentType.Invoice,
            Status = VendorInvoiceStatus.Open,
            InvoiceNumber = VendorInvoiceFaker.GenerateInvoiceNumber(new Faker()),
            Total = 213.61,
            Vendor = VendorHelper.ConvertToReadDto(DbContext.Vendors.First())
        };

        private async Task<string> PostInvoice(VendorInvoiceToWrite invoice)
        {
            {
                var json = JsonSerializer.Serialize(invoice, JsonSerializerHelper.DefaultSerializerOptions);
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
            var count = 2;
            var random = new Random();
            var paymentMethods = new VendorInvoicePaymentMethodFaker(false).Generate(count);
            DataSeeder.Save(paymentMethods);

            var defaultPaymentMethods = new DefaultPaymentMethodFaker(false).Generate(count)
                .Select(defaultPaymentMethod =>
                {
                    defaultPaymentMethod.NewPaymentMethod(paymentMethods[random.Next(paymentMethods.Count)]);
                    return defaultPaymentMethod;
                }).ToList();
            var vendors = new VendorFaker(false).Generate(count)
                .Select(vendor =>
                {
                    vendor.SetDefaultPaymentMethod(defaultPaymentMethods[random.Next(defaultPaymentMethods.Count)]);
                    return vendor;
                }).ToList();
            DataSeeder.Save(vendors);

            var saleCodeShopSupplies = new SaleCodeShopSuppliesFaker(false).Generate(count);
            var saleCodes = new SaleCodeFaker(false).Generate(count)
                .Select(saleCode =>
                {
                    saleCode.SetShopSupplies(saleCodeShopSupplies[0]);
                    return saleCode;
                }).ToList();
            DataSeeder.Save(saleCodes);

            var manufacturers = new ManufacturerFaker(false).Generate(count);
            DataSeeder.Save(manufacturers);

            var vendorInvoiceItems = new VendorInvoiceItemFaker(false).Generate(count)
                .Select(item =>
                {
                    item.NewManufacturer(manufacturers[random.Next(manufacturers.Count)]);
                    item.NewSaleCode(saleCodes[random.Next(saleCodes.Count)]);
                    return item;
                }).ToList();
            var salesTaxes = new SalesTaxFaker(false).Generate(count);
            var vendorInvoiceLineItems = new VendorInvoiceLineItemFaker(false).Generate(count);
            var vendorInvoiceTaxes = new VendorInvoiceTaxFaker(false).Generate(count)
                .Select(tax =>
                {
                    tax.SetSalesTax(salesTaxes[random.Next(salesTaxes.Count)]);
                    return tax;
                }).ToList();
            var vendorInvoicePayments = new VendorInvoicePaymentFaker(false).Generate(count);
            var invoices = new VendorInvoiceFaker(generateId: false, lineItemsCount: count, paymentsCount: count, taxesCount: count).Generate(count);

            DataSeeder.Save(invoices);
        }
    }
}
