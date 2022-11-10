using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using FluentAssertions;
using Menominee.Client.Components.Payables;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;

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
            invoiceTotals.BalanceForwards.Should().Be(0);
            invoiceTotals.Taxes.Should().Be(0);
            invoiceTotals.Total.Should().Be(0);
            invoiceTotals.Payments.Should().Be(0);
            invoiceTotals.TaxableTotal.Should().Be(0);
        }

        [Fact]
        public void Clear_Results_To_Zero()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var paymentLineCount = 5;
            var paymentLineAmount = 10.01;
            var taxLineCount = 5;
            var taxLineAmount = .1;
            var invoice = CreateVendorInvoiceToWrite(
                lineItemCount: lineItemCount,
                lineItemCore: lineItemCore,
                lineItemCost: lineItemCost,
                lineItemQuantity: lineItemQuantity,
                paymentLineCount: paymentLineCount,
                paymentLineAmount: paymentLineAmount,
                taxLineCount: taxLineCount,
                taxLineAmount: taxLineAmount);
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
            invoiceTotals.BalanceForwards.Should().Be(0);
            invoiceTotals.Taxes.Should().Be(0);
            invoiceTotals.Total.Should().Be(0);
            invoiceTotals.Payments.Should().Be(0);
            invoiceTotals.TaxableTotal.Should().Be(0);
        }

        [Fact]
        public void CalculateInvoiceTotals()
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
            var expectedTotal = 0.0;
            var invoice = CreateVendorInvoiceToWrite(
                lineItemCount: lineItemCount,
                lineItemCore: lineItemCore,
                lineItemCost: lineItemCost,
                lineItemQuantity: lineItemQuantity,
                paymentLineCount: paymentLineCount,
                paymentLineAmount: paymentLineAmount,
                taxLineCount: taxLineCount,
                taxLineAmount: taxLineAmount);
            var invoiceTotalsOrError = invoiceTotals.CalculateInvoiceTotals(invoice);


      //      Total = Purchases + Returns + CoreReturns + Defectives + Warranties
      //+ MiscellaneousDebits + MiscellaneousCredits + BalanceForwards + Taxes;


            invoiceTotalsOrError.Value.Payments.Should().Be(expectedPayments);
            invoiceTotalsOrError.Value.Taxes.Should().Be(expectedTaxes);
            invoiceTotalsOrError.Value.Total.Should().Be(expectedTotal);
            invoiceTotalsOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoicePayments()
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
            var invoice = CreateVendorInvoiceToWrite(
                lineItemCount: lineItemCount,
                lineItemCore: lineItemCore,
                lineItemCost: lineItemCost,
                lineItemQuantity: lineItemQuantity,
                paymentLineCount: paymentLineCount,
                paymentLineAmount: paymentLineAmount,
                taxLineCount: taxLineCount,
                taxLineAmount: taxLineAmount);

            var invoicePaymentsOrError = invoiceTotals.CalculateInvoicePayments(invoice);

            invoicePaymentsOrError.Value.Should().Be(expectedPayments);
            invoicePaymentsOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void CalculateInvoiceTaxes()
        {
            var invoiceTotals = new InvoiceTotals();
            var lineItemCount = 5;
            var lineItemCore = 1.0;
            var lineItemCost = 1.0;
            var lineItemQuantity = 10.0;
            var taxLineCount = 5;
            var taxLineAmount = 10.01;
            var expectedTaxes = taxLineCount * taxLineAmount;
            var invoice = CreateVendorInvoiceToWrite(
                lineItemCount: lineItemCount,
                lineItemCore: lineItemCore,
                lineItemCost: lineItemCost,
                lineItemQuantity: lineItemQuantity,
                0, 0, // payment
                taxLineCount: taxLineCount,
                taxLineAmount: taxLineAmount);

            var invoiceTaxesOrError = invoiceTotals.CalculateInvoiceTaxes(invoice);

            invoiceTaxesOrError.Value.Should().Be(expectedTaxes);
            invoiceTaxesOrError.IsFailure.Should().BeFalse();
        }

        private VendorInvoiceToWrite CreateVendorInvoiceToWrite(
            int lineItemCount,
            double lineItemCore,
            double lineItemCost,
            double lineItemQuantity,
            int paymentLineCount,
            double paymentLineAmount,
            int taxLineCount,
            double taxLineAmount)
        {
            return new()
            {
                Date = DateTime.Now,
                InvoiceNumber = "123",
                Vendor = CreateVendorToReadInList(),
                Status = VendorInvoiceStatus.Open,
                Total = 0,
                LineItems = CreateLineItems(lineItemCount, lineItemCore, lineItemCost, lineItemQuantity),
                Payments = CreatePayments(paymentLineCount, paymentLineAmount),
                Taxes = CreateTaxes(taxLineCount, taxLineAmount)
            };
        }

        private VendorToReadInList CreateVendorToReadInList()
        {
            return new VendorToReadInList()
            {
                Id = 1,
                IsActive = true,
                Name = Utilities.RandomCharacters(Vendor.MinimumLength) + 1,
                VendorCode = Utilities.RandomCharacters(Vendor.MinimumLength + 1)
            };
        }

        private IList<VendorInvoiceTaxToWrite> CreateTaxes(int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount
                });
            }

            return taxes;
        }

        private IList<VendorInvoicePaymentToWrite> CreatePayments(int paymentCount, double paymentAmount)
        {
            var payments = new List<VendorInvoicePaymentToWrite>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = paymentAmount
                });
            }

            return payments;
        }

        private static IList<VendorInvoiceLineItemToWrite> CreateLineItems(int lineItemCount, double core, double cost, double lineItemQuantity)
        {
            var lineItems = new List<VendorInvoiceLineItemToWrite>();

            for (int i = 0; i < lineItemCount; i++)
            {
                lineItems.Add(
                    new VendorInvoiceLineItemToWrite()
                    {
                        Core = core,
                        Cost = cost,
                        Quantity = lineItemQuantity,
                        PONumber = String.Empty,
                        Type = VendorInvoiceLineItemType.Purchase
                    });
            }

            return lineItems;
        }
    }
}
