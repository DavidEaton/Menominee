using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.Payables.Invoices;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Api.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Taxes;
using FluentAssertions;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Helpers.VendorInvoiceHelper;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    public class VendorInvoicesControllerShould : IntegrationTestBase
    {
        [Fact]
        public async Task AddInvoiceAsync()
        {
            var invoiceToAdd = CreateTestVendorInvoice();

            // Arrange: Create/Add LineItems
            VendorInvoiceLineItemType lineItemType = VendorInvoiceLineItemType.Purchase;
            var lineItemCount = 5;
            var lineItemCore = 2.2;
            var lineItemCost = 4.4;
            var lineItemQuantity = 2;

            invoiceToAdd.LineItems = CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);

            // Arrange: Create/Add Payments
            IReadOnlyList<string> paymentMethodNames = new List<string>();
            var paymentMethod = new VendorInvoicePaymentMethodToRead();

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var paymentMethodRepository = new VendorInvoicePaymentMethodRepository(context);
                IReadOnlyList<VendorInvoicePaymentMethodToRead> paymentMethods = await paymentMethodRepository.GetPaymentMethodsAsync();
                paymentMethod = paymentMethods[paymentMethods.Count - 1];
                paymentMethodNames = await paymentMethodRepository.GetPaymentMethodNamesAsync();
            }

            var paymentLineCount = 2;
            var paymentLineAmount = 3.3;

            invoiceToAdd.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, paymentMethod);

            // Arrange: Create/Add Taxes
            var salesTax = new SalesTaxToRead();
            var amount = 1.55;
            var taxLineCount = 3;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var salesTaxRepository = new SalesTaxRepository(context);
                var taxes = await salesTaxRepository.GetSalesTaxesAsync();
                salesTax = taxes[taxes.Count - 1];
            }

            invoiceToAdd.Taxes = CreateTaxesToWrite(salesTax, taxLineCount, amount);

            // Act
            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);

                var addInvoiceResult = await controller.AddInvoiceAsync(invoiceToAdd);

                var addInvoiceResponse = JsonSerializer.Deserialize<CreatedResultResponse>(addInvoiceResult.ToJson());
                var getInvoiceActionResult = await controller.GetInvoiceAsync(addInvoiceResponse.Value.Id);
                var actionResultValue = (OkObjectResult)getInvoiceActionResult.Result;
                var vendorInvoice = (VendorInvoiceToRead)actionResultValue.Value;

                // Assert
                vendorInvoice.Should().BeOfType<VendorInvoiceToRead>();
                vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
                vendorInvoice.Date.Should().Be(DateTime.Today);
                vendorInvoice.DatePosted.Should().Be(null);
                vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
                vendorInvoice.Payments.Count.Should().Be(paymentLineCount);
                vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
            }
        }

        [Fact]
        public async Task UpdateInvoiceAsync()
        {
            var invoiceToAdd = CreateTestVendorInvoice();

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var getInvoiceActionResult = await controller.GetInvoiceAsync(1);
                var actionResultValue = (OkObjectResult)getInvoiceActionResult.Result;
                var vendorInvoice = (VendorInvoiceToRead)actionResultValue.Value;

                //vendorInvoice.Should().BeOfType<VendorInvoiceToRead>();
                //vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
                //vendorInvoice.Date.Should().Be(DateTime.Today);
                //vendorInvoice.DatePosted.Should().Be(null);
                //vendorInvoice.LineItems.Count.Should().Be(0);
                //vendorInvoice.Payments.Count.Should().Be(0);
                //vendorInvoice.Taxes.Count.Should().Be(0);

                true.Should().BeFalse();
            }
        }

        private static VendorInvoiceToWrite CreateTestVendorInvoice()
        {
            // Arrange: Create Invoice
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = new VendorToReadInList()
                {
                    Id = 1,
                    IsActive = true,
                    Name = "Vendor One",
                    VendorCode = "V01"
                }
            };
        }

        private static VendorInvoicesController CreateController(ApplicationDbContext context)
        {
            return new VendorInvoicesController(
                                new VendorInvoiceRepository(context),
                                new VendorRepository(context),
                                new VendorInvoicePaymentMethodRepository(context),
                                new SalesTaxRepository(context),
                                new ManufacturerRepository(context),
                                new SaleCodeRepository(context)
                                );
        }
    }
}
