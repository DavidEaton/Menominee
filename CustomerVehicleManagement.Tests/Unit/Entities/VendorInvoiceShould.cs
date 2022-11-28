using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Helpers.VendorInvoiceHelper;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class VendorInvoiceShould
    {
        [Fact]
        public void Create_VendorInvoice()
        {
            // Arrange
            var vendorOrError = Vendor.Create("Vendor One", "V1");
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendorOrError.Value);

            // Act
            var vendorInvoiceOrError = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: "");

            // Assert
            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_InvoiceNumber()
        {
            var vendorOneOrError = Vendor.Create("Vendor One", "V1");
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendorOneOrError.Value);
            var invoiceNumber = "123456";

            var vendorInvoice = VendorInvoice.Create(
                vendorOneOrError.Value,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: invoiceNumber);

            vendorInvoice.Value.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_DatePosted()
        {
            var datePosted = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                datePosted: datePosted);

            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Should().NotBeNull();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_Date()
        {
            var date = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: date);

            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Should().NotBeNull();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Open_Status()
        {
            var vendorOneOrError = Vendor.Create("Vendor One", "V1");
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendorOneOrError.Value);

            var vendorInvoice = VendorInvoice.Create(
                vendorOneOrError.Value,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoice.Value.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Null_Vendor()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor: null,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                invalidStatus,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_DocumentType()
        {
            var invalidDocumentType = (VendorInvoiceDocumentType)(-1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                invalidDocumentType,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Total()
        {
            var invalidTotal = -1;
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                invalidTotal,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Date()
        {
            var invalidDate = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: invalidDate);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_DatePosted()
        {
            var date = DateTime.Today;
            var invalidDatePosted = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: date,
                datePosted: invalidDatePosted);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_InvoiceNumber()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: invalidInvoiceNumber);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Nonunique_Vendor_InvoiceNumber()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var invoiceNumbers = new List<int>() { 1, 2, 3, 4 };
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbers(vendor, invoiceNumbers);

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: $"{vendor.Id}{2}");

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
            vendorInvoiceOrError.Error.Should().Contain("unique");
        }

        [Fact]
        public void AddLineItem()
        {
            var vendorInvoice = CreateVendorInvoice();

            var lineItemType = VendorInvoiceLineItemType.Purchase;
            int lineItemCount = 5;
            double lineItemCore = 2.2;
            double lineItemCost = 4.4;
            double lineItemQuantity = 2;

            var lineItems = CreateLineItems(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            foreach (var lineItem in lineItems)
                vendorInvoice.AddLineItem(lineItem);

            vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
        }

        [Fact]
        public void RemoveLineItem()
        {
            var vendorInvoice = CreateVendorInvoice();

            int lineItemCount = 5;
            var lineItems = CreateLineItems(
                type: VendorInvoiceLineItemType.Purchase,
                lineItemCount: lineItemCount,
                core: 2.2,
                cost: 4.4,
                itemQuantity: 2);
            foreach (var lineItem in lineItems)
                vendorInvoice.AddLineItem(lineItem);
            vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
            var lineItemToRemove = vendorInvoice.LineItems[1];

            vendorInvoice.RemoveLineItem(lineItemToRemove);

            vendorInvoice.LineItems.Count.Should().Be(lineItemCount - 1);
        }

        [Fact]
        public void AddPayment()
        {
            var vendorInvoice = CreateVendorInvoice();
            var paymentCount = 5;
            var payments = CreatePayments(paymentCount: paymentCount, paymentAmount: 1);

            foreach (var payment in payments)
                vendorInvoice.AddPayment(payment);

            vendorInvoice.Payments.Count.Should().Be(paymentCount);
        }

        [Fact]
        public void RemovePayment()
        {
            var vendorInvoice = CreateVendorInvoice();
            var paymentCount = 5;
            var payments = CreatePayments(paymentCount: paymentCount, paymentAmount: 1);
            foreach (var payment in payments)
                vendorInvoice.AddPayment(payment);
            vendorInvoice.Payments.Count.Should().Be(paymentCount);
            var paymentToRemove = vendorInvoice.Payments[1];

            vendorInvoice.RemovePayment(paymentToRemove);
            vendorInvoice.Payments.Count.Should().Be(paymentCount - 1);
        }

        [Fact]
        public void AddTax()
        {
            var vendorInvoice = CreateVendorInvoice();
            var taxLineCount = 5;
            var taxes = CreateTaxes(taxLineCount: taxLineCount, taxAmount: 1);

            foreach (var tax in taxes)
                vendorInvoice.AddTax(tax);

            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
        }

        [Fact]
        public void RemoveTax()
        {
            var vendorInvoice = CreateVendorInvoice();
            var taxLineCount = 5;
            var taxes = CreateTaxes(taxLineCount: taxLineCount, taxAmount: 1);
            foreach (var tax in taxes)
                vendorInvoice.AddTax(tax);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
            var taxToRemove = vendorInvoice.Taxes[1];

            vendorInvoice.RemoveTax(taxToRemove);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount - 1);
        }

        [Fact]
        public void SetVendor()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendorOne);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var vendorTwo = Vendor.Create("Vendor Two", "V@").Value;
            vendorInvoice.SetVendor(vendorTwo);

            vendorInvoice.Vendor.Should().Be(vendorTwo);
        }

        [Fact]
        public void SetVendorInvoiceStatus()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;
            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);

            vendorInvoice.SetVendorInvoiceStatus(VendorInvoiceStatus.Reconciled);

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Reconciled);
        }

        [Fact]
        public void SetInvoiceNumber()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var invoiceNumber = "001";
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                invoiceNumber: invoiceNumber,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
            var newInvoiceNumber = "002";
            vendorInvoice.SetInvoiceNumber(newInvoiceNumber, vendorInvoiceNumbers);

            vendorInvoice.InvoiceNumber.Should().Be(newInvoiceNumber);
        }

        [Fact]
        public void SetVendorInvoiceDocumentType()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var invoiceNumber = "001";
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                invoiceNumber: invoiceNumber,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.DocumentType.Should().Be(VendorInvoiceDocumentType.Unknown);
            var newInvoiceNumber = VendorInvoiceDocumentType.Invoice;
            var resultOrError = vendorInvoice.SetVendorInvoiceDocumentType(newInvoiceNumber);

            resultOrError.Value.Should().Be(newInvoiceNumber);
        }

        [Fact]
        public void SetTotal()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var total = 1;
            var newTotal = 2;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            vendorInvoice.Total.Should().Be(total);
            vendorInvoice.SetTotal(newTotal);

            vendorInvoice.Total.Should().Be(newTotal);
        }

        [Fact]
        public void SetDate()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            vendorInvoice.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDate(date);

            vendorInvoice.Date.Should().Be(date);
        }

        [Fact]
        public void ClearDate()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            DateTime? date = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                date: date,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            vendorInvoice.Date.Should().NotBeNull();
            vendorInvoice.ClearDate();

            vendorInvoice.Date.Should().BeNull();
        }

        [Fact]
        public void SetDatePosted()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            DateTime? date = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            vendorInvoice.DatePosted.Should().BeNull();
            vendorInvoice.SetDatePosted(date);

            vendorInvoice.DatePosted.Should().Be(date);
        }

        [Fact]
        public void ClearDatePosted()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            DateTime? datePosted = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                datePosted: datePosted,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            vendorInvoice.DatePosted.Should().NotBeNull();
            vendorInvoice.ClearDatePosted();

            vendorInvoice.DatePosted.Should().BeNull();
        }

        [Fact]
        public void Not_Set_Null_Vendor()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetVendor(vendor: null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_VendorInvoice_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetVendorInvoiceStatus(invalidStatus);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Invoice_Number()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetInvoiceNumber(invalidInvoiceNumber, vendorInvoiceNumbers);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_VendorInvoiceDocumentType()
        {
            var invalidDocumentType = (VendorInvoiceDocumentType)(-1);

            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.DocumentType.Should().Be(VendorInvoiceDocumentType.Unknown);
            var resultOrError = vendorInvoice.SetVendorInvoiceDocumentType(invalidDocumentType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Set_Null_Invoice_Number()
        {
            string nullInvoiceNumber = null;
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetInvoiceNumber(nullInvoiceNumber, vendorInvoiceNumbers);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Total()
        {
            var invalidTotal = VendorInvoice.MinimumValue - 1;
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetTotal(invalidTotal);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Date(DateTime? date)
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetDate(date);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_DatePosted(DateTime? datePosted)
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetDatePosted(datePosted);

            resultOrError.IsFailure.Should().BeTrue();
        }


        [Fact]
        public void Not_Set_Nonunique_InvoiceNumber()
        {
            var vendor = Vendor.Create("Vendor One", "V1").Value;
            var invoiceNumber = "05";
            var invoiceNumbers = new List<int>() { 1, 2, 3, 4 };
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbers(vendor, invoiceNumbers);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                invoiceNumber: invoiceNumber,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.InvoiceNumber.Should().Be(invoiceNumber);
            var NonuniqueInvoiceNumber = "02";
            var resultOrError = vendorInvoice.SetInvoiceNumber(NonuniqueInvoiceNumber, vendorInvoiceNumbers);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().NotBeNull();
            resultOrError.Error.Should().Contain("unique");
        }

        public class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { null };
                    yield return new object[] { DateTime.Today.AddDays(1) };
                }
            }
        }
    }
}
