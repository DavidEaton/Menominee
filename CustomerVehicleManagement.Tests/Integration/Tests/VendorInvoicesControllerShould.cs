using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.Payables.Invoices;
using CustomerVehicleManagement.Api.Payables.PaymentMethods;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Api.Taxes;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using FluentAssertions;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Helpers.Payables.VendorInvoiceTestHelper;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    public class VendorInvoicesControllerShould : IntegrationTestBase
    {
        [Fact]
        public async Task AddInvoiceAsync()
        {
            var vendorToRead = await CreateVendorToRead();

            var invoiceToAdd = CreateVendorInvoiceToWrite(vendorToRead);

            // Arrange: Create/Add LineItems
            VendorInvoiceLineItemType lineItemType = VendorInvoiceLineItemType.Purchase;
            var lineItemCount = 5;
            var lineItemCore = 2.2;
            var lineItemCost = 4.4;
            var lineItemQuantity = 2;

            invoiceToAdd.LineItems = CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);

            // Arrange: Create/Add Payments
            VendorInvoicePaymentMethod paymentMethod = null;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                paymentMethod = context.VendorInvoicePaymentMethods.FirstOrDefault();
            }

            var paymentLineCount = 2;
            var paymentLineAmount = 3.3;

            invoiceToAdd.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, paymentMethod);

            // Arrange: Create/Add Taxes
            SalesTax salesTax = null;
            var amount = 1.55;
            var taxLineCount = 3;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var salesTaxRepository = new SalesTaxRepository(context);
                salesTax = context.SalesTaxes.FirstOrDefault();
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

        private static async Task<VendorToRead> CreateVendorToRead()
        {
            IReadOnlyList<VendorToRead> vendors = new List<VendorToRead>();
            VendorToRead vendorToRead = null;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var vendorRepository = new VendorRepository(context);
                vendors = await vendorRepository.GetVendorsAsync();
                vendorToRead = vendors.FirstOrDefault();
            }

            return vendorToRead;
        }

        [Fact]
        public async Task UpdateInvoiceAsync()
        {
            IReadOnlyList<VendorToRead> vendors = new List<VendorToRead>();
            VendorToRead vendorToRead = null;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var vendorRepository = new VendorRepository(context);
                vendors = await vendorRepository.GetVendorsAsync();
                vendorToRead = vendors.FirstOrDefault();
            }

            var invoiceToAdd = CreateVendorInvoiceToWrite(vendorToRead);

            // Arrange: Create/Add LineItems
            VendorInvoiceLineItemType lineItemType = VendorInvoiceLineItemType.Purchase;
            var lineItemCount = 5;
            var lineItemCore = 2.2;
            var lineItemCost = 4.4;
            var lineItemQuantity = 2;

            invoiceToAdd.LineItems = CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);

            // Arrange: Create/Add Payments
            VendorInvoicePaymentMethod paymentMethod = null;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                paymentMethod = context.VendorInvoicePaymentMethods.FirstOrDefault();
            }

            var paymentLineCount = 2;
            var paymentLineAmount = 3.3;

            invoiceToAdd.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, paymentMethod);

            // Arrange: Create/Add Taxes
            SalesTax salesTax = null;
            var amount = 1.55;
            var taxLineCount = 3;

            using (var context = Helpers.CreateTestContext())
            {
                var controller = CreateController(context);
                var salesTaxRepository = new SalesTaxRepository(context);
                salesTax = context.SalesTaxes.FirstOrDefault();
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
