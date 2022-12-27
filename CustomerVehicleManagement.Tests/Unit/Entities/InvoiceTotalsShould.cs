using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Taxes;
using CustomerVehicleManagement.Tests.Unit.Helpers.Payables;
using FluentAssertions;
using Menominee.Client.Components.Payables;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Helpers.Payables.VendorInvoiceTestHelper;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InvoiceTotalsShould
    {
        [Fact]
        public void Initialize_Values_To_Zero()
        {
            var invoiceTotals = new InvoiceTotals();

            invoiceTotals.Purchases.Should().Be(0);
            invoiceTotals.Returns.Should().Be(0);
            invoiceTotals.CoreReturns.Should().Be(0);
            invoiceTotals.Defectives.Should().Be(0);
            invoiceTotals.Warranties.Should().Be(0);
            invoiceTotals.MiscellaneousDebits.Should().Be(0);
            invoiceTotals.MiscellaneousCredits.Should().Be(0);
            invoiceTotals.BalancesForward.Should().Be(0);
            invoiceTotals.Taxes.Should().Be(0);
            invoiceTotals.Total.Should().Be(0);
            invoiceTotals.Payments.Should().Be(0);
            invoiceTotals.TaxableTotal.Should().Be(0);
        }

        [Fact]
        public void Clear_Results_To_Zero()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemType = VendorInvoiceLineItemType.Purchase;
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var invoice = CreateVendorInvoiceToWrite(CreateVendor());
            invoice.LineItems = CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            invoice.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, CreateVendorInvoicePaymentMethodToRead());
            var invoiceTotalsOrError = invoiceTotals.CalculateInvoiceTotals(invoice);
            invoiceTotalsOrError.Value.Total.Should().BeGreaterThan(0);
            invoiceTotalsOrError.IsFailure.Should().BeFalse();

            invoiceTotals.Clear();

            invoiceTotals.Purchases.Should().Be(0);
            invoiceTotals.Returns.Should().Be(0);
            invoiceTotals.CoreReturns.Should().Be(0);
            invoiceTotals.Defectives.Should().Be(0);
            invoiceTotals.Warranties.Should().Be(0);
            invoiceTotals.MiscellaneousDebits.Should().Be(0);
            invoiceTotals.MiscellaneousCredits.Should().Be(0);
            invoiceTotals.BalancesForward.Should().Be(0);
            invoiceTotals.Taxes.Should().Be(0);
            invoiceTotals.Total.Should().Be(0);
            invoiceTotals.Payments.Should().Be(0);
            invoiceTotals.TaxableTotal.Should().Be(0);
        }

        private VendorInvoicePaymentMethodToRead CreateVendorInvoicePaymentMethodToRead()
        {
            return new()
            {
                Id = int.MaxValue,
                IsActive = true,
                PaymentType = VendorInvoicePaymentMethodType.Normal,
                Name = LoremIpsum(10)
            };
        }

        [Fact]
        public void CalculateInvoiceTotal()
        {
            var invoiceTotals = new InvoiceTotals();
            var type = VendorInvoiceLineItemType.Purchase;
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var expectedPayments = paymentLineCount * paymentLineAmount;
            var taxLineCount = 5;
            var taxLineAmount = .1;
            var expectedTaxes = taxLineCount * taxLineAmount;
            var expectedPurchasesAmount = type == VendorInvoiceLineItemType.Purchase
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedReturnsAmount = type == VendorInvoiceLineItemType.Return
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedCoreReturnsAmount = type == VendorInvoiceLineItemType.CoreReturn
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedDefectivesAmount = type == VendorInvoiceLineItemType.Defective
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedWarrantiesAmount = type == VendorInvoiceLineItemType.Warranty
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedMiscellaneousDebitsAmount = type == VendorInvoiceLineItemType.MiscDebit
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedMiscellaneousCreditsAmount = type == VendorInvoiceLineItemType.MiscCredit
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedBalanceForwardsAmount = type == VendorInvoiceLineItemType.BalanceForward
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedTotal =
                  expectedPurchasesAmount
                + expectedReturnsAmount
                + expectedCoreReturnsAmount
                + expectedDefectivesAmount
                + expectedWarrantiesAmount
                + expectedMiscellaneousDebitsAmount
                + expectedMiscellaneousCreditsAmount
                + expectedBalanceForwardsAmount
                + expectedTaxes;

            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(type, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            invoice.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, CreatePaymentMethodToRead());
            invoice.Taxes = CreateTaxesToWrite(CreateSalesTaxToRead(), taxLineCount: taxLineCount, taxAmount: taxLineAmount);
            var invoiceTotalsOrError = invoiceTotals.CalculateInvoiceTotals(invoice);

            invoiceTotalsOrError.IsFailure.Should().BeFalse();
            invoiceTotalsOrError.Value.Payments.Should().Be(expectedPayments);
            invoiceTotalsOrError.Value.Taxes.Should().Be(expectedTaxes);
            invoiceTotalsOrError.Value.Purchases.Should().Be(expectedPurchasesAmount);
            invoiceTotalsOrError.Value.Returns.Should().Be(expectedReturnsAmount);
            invoiceTotalsOrError.Value.CoreReturns.Should().Be(expectedCoreReturnsAmount);
            invoiceTotalsOrError.Value.Defectives.Should().Be(expectedDefectivesAmount);
            invoiceTotalsOrError.Value.Warranties.Should().Be(expectedWarrantiesAmount);
            invoiceTotalsOrError.Value.MiscellaneousDebits.Should().Be(expectedMiscellaneousDebitsAmount);
            invoiceTotalsOrError.Value.MiscellaneousCredits.Should().Be(expectedMiscellaneousCreditsAmount);
            invoiceTotalsOrError.Value.BalancesForward.Should().Be(expectedBalanceForwardsAmount);
            invoiceTotalsOrError.Value.Total.Should().Be(expectedTotal);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void CalculateInvoiceTotals_For_Each_LineItemType(VendorInvoiceLineItemType lineItemType)
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var expectedPayments = paymentLineCount * paymentLineAmount;
            var taxLineCount = 5;
            var taxLineAmount = .1;
            var expectedTaxes = taxLineCount * taxLineAmount;
            var expectedPurchasesAmount = lineItemType == VendorInvoiceLineItemType.Purchase
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedReturnsAmount = lineItemType == VendorInvoiceLineItemType.Return
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedCoreReturnsAmount = lineItemType == VendorInvoiceLineItemType.CoreReturn
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedDefectivesAmount = lineItemType == VendorInvoiceLineItemType.Defective
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedWarrantiesAmount = lineItemType == VendorInvoiceLineItemType.Warranty
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedMiscellaneousDebitsAmount = lineItemType == VendorInvoiceLineItemType.MiscDebit
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedMiscellaneousCreditsAmount = lineItemType == VendorInvoiceLineItemType.MiscCredit
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedBalanceForwardsAmount = lineItemType == VendorInvoiceLineItemType.BalanceForward
                ? (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount
                : 0;
            var expectedTotal =
                  expectedPurchasesAmount
                + expectedReturnsAmount
                + expectedCoreReturnsAmount
                + expectedDefectivesAmount
                + expectedWarrantiesAmount
                + expectedMiscellaneousDebitsAmount
                + expectedMiscellaneousCreditsAmount
                + expectedBalanceForwardsAmount
                + expectedTaxes;

            var options = new LineItemTestOptions
            {
                Type = lineItemType
            };
            var invoiceToWrite = CreateVendorInvoiceToWrite(CreateVendorToRead());
            invoiceToWrite.LineItems = CreateLineItemsToWrite(options);
            invoiceToWrite.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, CreatePaymentMethodToRead());
            invoiceToWrite.Taxes = CreateTaxesToWrite(CreateSalesTaxToRead(), taxLineCount: taxLineCount, taxAmount: taxLineAmount);
            var invoiceTotalsOrError = invoiceTotals.CalculateInvoiceTotals(invoiceToWrite);

            invoiceTotalsOrError.IsFailure.Should().BeFalse();
            invoiceTotalsOrError.Value.Payments.Should().Be(expectedPayments);
            invoiceTotalsOrError.Value.Taxes.Should().Be(expectedTaxes);
            invoiceTotalsOrError.Value.Purchases.Should().Be(expectedPurchasesAmount);
            invoiceTotalsOrError.Value.Returns.Should().Be(expectedReturnsAmount);
            invoiceTotalsOrError.Value.CoreReturns.Should().Be(expectedCoreReturnsAmount);
            invoiceTotalsOrError.Value.Defectives.Should().Be(expectedDefectivesAmount);
            invoiceTotalsOrError.Value.Warranties.Should().Be(expectedWarrantiesAmount);
            invoiceTotalsOrError.Value.MiscellaneousDebits.Should().Be(expectedMiscellaneousDebitsAmount);
            invoiceTotalsOrError.Value.MiscellaneousCredits.Should().Be(expectedMiscellaneousCreditsAmount);
            invoiceTotalsOrError.Value.BalancesForward.Should().Be(expectedBalanceForwardsAmount);
            invoiceTotalsOrError.Value.Total.Should().Be(expectedTotal);
        }

        private static VendorInvoicePaymentMethodToRead CreatePaymentMethodToRead()
        {
            return new()
            {
                Id = 1,
                IsActive = true,
                PaymentType = VendorInvoicePaymentMethodType.Normal,
                Name = RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1)
            };
        }

        [Fact]
        public void CalculateInvoicePayments()
        {
            var options = new LineItemTestOptions
            {
                RowCount = 5
            };
            var invoiceTotals = new InvoiceTotals();
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var expectedPayments = paymentLineCount * paymentLineAmount;
            var taxLineCount = 5;
            var taxLineAmount = .1;
            var invoiceToWrite = CreateVendorInvoiceToWrite(CreateVendorToRead());
            invoiceToWrite.LineItems = CreateLineItemsToWrite(options);
            invoiceToWrite.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, CreatePaymentMethodToRead());
            invoiceToWrite.Taxes = CreateTaxesToWrite(CreateSalesTaxToRead(), taxLineCount: taxLineCount, taxAmount: taxLineAmount);

            var invoicePaymentsOrError = invoiceTotals.CalculateInvoicePayments(invoiceToWrite);

            invoicePaymentsOrError.Value.Should().Be(expectedPayments);
            invoicePaymentsOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceTaxes()
        {
            var invoiceTotals = new InvoiceTotals();
            var taxLineCount = 5;
            var taxLineAmount = 10.01;
            var expectedTaxes = taxLineCount * taxLineAmount;
            var invoice = CreateVendorInvoiceToWrite(CreateVendor());
            invoice.Taxes = CreateTaxesToWrite(CreateSalesTax(), taxLineCount, taxLineAmount);

            var invoiceTaxesOrError = invoiceTotals.CalculateInvoiceTaxes(invoice);

            invoiceTaxesOrError.Value.Should().Be(expectedTaxes);
            invoiceTaxesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoicePurchases()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite(CreateVendor());
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.Purchase, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedPurchaseAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoicePurchasesOrError = invoiceTotals.CalculateInvoicePurchases(invoice);

            invoicePurchasesOrError.Value.Should().Be(expectedPurchaseAmount);
            invoicePurchasesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceReturns()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.Return, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedReturnAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceReturnsOrError = invoiceTotals.CalculateInvoiceReturns(invoice);

            invoiceReturnsOrError.Value.Should().Be(expectedReturnAmount);
            invoiceReturnsOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceCoreReturns()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.CoreReturn, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedCoreReturnAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceCoreReturnsOrError = invoiceTotals.CalculateInvoiceCoreReturns(invoice);

            invoiceCoreReturnsOrError.Value.Should().Be(expectedCoreReturnAmount);
            invoiceCoreReturnsOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceDefectives()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.Defective, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedDefectiveAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceDefectivesOrError = invoiceTotals.CalculateInvoiceDefectives(invoice);

            invoiceDefectivesOrError.Value.Should().Be(expectedDefectiveAmount);
            invoiceDefectivesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceWarranties()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.Warranty, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedWarrantyAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceWarrantiesOrError = invoiceTotals.CalculateInvoiceWarranties(invoice);

            invoiceWarrantiesOrError.Value.Should().Be(expectedWarrantyAmount);
            invoiceWarrantiesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceMiscellaneousDebits()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.MiscDebit, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedMiscellaneousDebitAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceWarrantiesOrError = invoiceTotals.CalculateInvoiceMiscellaneousDebits(invoice);

            invoiceWarrantiesOrError.Value.Should().Be(expectedMiscellaneousDebitAmount);
            invoiceWarrantiesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceMiscellaneousCredits()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.MiscCredit, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedMiscellaneousCreditAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceWarrantiesOrError = invoiceTotals.CalculateInvoiceMiscellaneousCredits(invoice);

            invoiceWarrantiesOrError.Value.Should().Be(expectedMiscellaneousCreditAmount);
            invoiceWarrantiesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceBalanceForwards()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.BalanceForward, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            var expectedBalanceForwardAmount = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;

            var invoiceWarrantiesOrError = invoiceTotals.CalculateInvoiceBalanceForwards(invoice);

            invoiceWarrantiesOrError.Value.Should().Be(expectedBalanceForwardAmount);
            invoiceWarrantiesOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceTaxableTotal()
        {
            var invoiceTotals = new InvoiceTotals();
            // Purchase LineItems: 100
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var lineItemType = VendorInvoiceLineItemType.Purchase;
            var expectedPurchases = (lineItemCost + lineItemCore) * lineItemQuantity * lineItemCount;
            // Payments: 50.05
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var expectedPayments = paymentLineCount * paymentLineAmount;
            // Taxes: .5
            var taxLineCount = 5;
            var taxLineAmount = .1;
            var expectedTaxes = taxLineCount * taxLineAmount;
            // BalancesForwarded: 4
            var balanceForwardLineItemCount = 2;
            var balanceForwardLineItemCore = 1.0;
            var balanceForwardLineItemCost = 1.0;
            var balanceForwardLineItemQuantity = 1.0;
            var expectedBalanceForwardAmount =
                (balanceForwardLineItemCost + balanceForwardLineItemCore) * balanceForwardLineItemQuantity * balanceForwardLineItemCount;
            // Total: 104.5
            var expectedTotal =
                // Purchase LineItems: 100
                (lineItemCost + lineItemCore)
                * lineItemQuantity
                * lineItemCount
                // Taxes: .5
                + (taxLineCount * taxLineAmount)
                // BalanceForwards: 4
                + (balanceForwardLineItemCost + balanceForwardLineItemCore) * balanceForwardLineItemQuantity * balanceForwardLineItemCount;

            var balanceForwardLineItems = CreateLineItemsToWrite(VendorInvoiceLineItemType.BalanceForward, balanceForwardLineItemCount, balanceForwardLineItemCore, balanceForwardLineItemCost, balanceForwardLineItemQuantity);

            var invoice = CreateVendorInvoiceToWrite();
            invoice.LineItems = CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            invoice.Payments = CreatePaymentsToWrite(paymentLineCount, paymentLineAmount, CreatePaymentMethodToRead());
            invoice.Taxes = CreateTaxesToWrite(CreateSalesTaxToRead(), taxLineCount: taxLineCount, taxAmount: taxLineAmount);


            // add the balanceForwardLineItems to the invoice
            foreach (var item in balanceForwardLineItems)
                invoice.LineItems.Add(item);
            // Recalculate invoice values
            var invoiceTaxableTotalOrError = invoiceTotals.CalculateInvoiceTaxableTotal(invoice);

            // TaxableTotal = Total - BalanceForwards - Taxes;
            //  100                           104.5           4                              .5
            var expectedInvoiceTaxableTotal = expectedTotal - expectedBalanceForwardAmount - expectedTaxes;

            invoiceTotals.Taxes.Should().Be(expectedTaxes);
            invoiceTotals.Payments.Should().Be(expectedPayments);
            invoiceTotals.Purchases.Should().Be(expectedPurchases);
            invoiceTotals.BalancesForward.Should().Be(expectedBalanceForwardAmount);
            invoiceTaxableTotalOrError.Value.Should().Be(expectedInvoiceTaxableTotal);
            invoiceTaxableTotalOrError.IsFailure.Should().BeFalse();
        }

        private static VendorToRead CreateVendorToRead()
        {
            return new VendorToRead()
            {
                Id = 1,
                IsActive = true,
                Name = RandomCharacters(Vendor.MinimumLength) + 1,
                VendorCode = RandomCharacters(Vendor.MinimumLength + 1)
            };

        }
        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { VendorInvoiceLineItemType.BalanceForward };
                    yield return new object[] { VendorInvoiceLineItemType.CoreReturn };
                    yield return new object[] { VendorInvoiceLineItemType.Defective };
                    yield return new object[] { VendorInvoiceLineItemType.MiscCredit };
                    yield return new object[] { VendorInvoiceLineItemType.MiscDebit };
                    yield return new object[] { VendorInvoiceLineItemType.Purchase };
                    yield return new object[] { VendorInvoiceLineItemType.Return };
                    yield return new object[] { VendorInvoiceLineItemType.Warranty };
                }
            }
        }

    }
}
