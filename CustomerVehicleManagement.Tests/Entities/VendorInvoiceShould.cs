using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using TestingHelperLibrary.Fakers;
using TestingHelperLibrary.Payables;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class VendorInvoiceShould
    {
        [Fact]
        public void Create_VendorInvoice()
        {
            // Arrange
            var vendorOrError = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier);
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendorOrError.Value);

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
            var vendorOrError = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier);
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendorOrError.Value);
            var invoiceNumber = "123456";

            var vendorInvoice = VendorInvoice.Create(
                vendorOrError.Value,
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendorOrError = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier);
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendorOrError.Value);

            var vendorInvoice = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            vendorInvoice.Value.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Null_Vendor()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var invoiceNumbers = new List<int>() { 1, 2, 3, 4 };
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbers(vendor, invoiceNumbers);

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
            int lineItemCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, lineItemCount);
            var lineItemType = VendorInvoiceLineItemType.Purchase;
            double lineItemCore = 2.2;
            double lineItemCost = 4.4;
            double lineItemQuantity = 2;

            var lineItems = VendorInvoiceTestHelper.CreateLineItems(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
            foreach (var lineItem in lineItems)
                vendorInvoice.AddLineItem(lineItem);

            vendorInvoice.LineItems.Count.Should().Be(lineItemCount + lineItems.Count);
        }

        [Fact]
        public void Not_Add_Null_LineItem()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, 0);
            VendorInvoiceLineItem nullLineItem = null;

            var result = vendorInvoice.AddLineItem(nullLineItem);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
            vendorInvoice.LineItems.Count.Should().Be(0);
        }

        [Fact]
        public void RemoveLineItem()
        {
            int lineItemCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, lineItemCount);
            vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
            var lineItemToRemove = vendorInvoice.LineItems[1];

            vendorInvoice.RemoveLineItem(lineItemToRemove);

            vendorInvoice.LineItems.Count.Should().Be(lineItemCount - 1);
        }

        [Fact]
        public void Not_Remove_Null_LineItem()
        {
            int lineItemCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, lineItemCount);
            vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
            VendorInvoiceLineItem nullLineItem = null;

            var result = vendorInvoice.RemoveLineItem(nullLineItem);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
            vendorInvoice.LineItems.Count.Should().Be(lineItemCount);
        }

        [Fact]
        public void AddPayment()
        {
            var paymentCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, 0);
            var payments = VendorInvoiceTestHelper.CreatePayments(paymentCount: paymentCount, paymentAmount: 1);

            foreach (var payment in payments)
                vendorInvoice.AddPayment(payment);

            vendorInvoice.Payments.Count.Should().Be(paymentCount);
        }

        [Fact]
        public void Not_Add_Null_Payment()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, 0);
            VendorInvoicePayment nullPayment = null;

            var result = vendorInvoice.AddPayment(nullPayment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
        }

        [Fact]
        public void RemovePayment()
        {
            var paymentCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, paymentCount);
            vendorInvoice.Payments.Count.Should().Be(paymentCount);
            var paymentToRemove = vendorInvoice.Payments[1];

            vendorInvoice.RemovePayment(paymentToRemove);
            vendorInvoice.Payments.Count.Should().Be(paymentCount - 1);
        }

        [Fact]
        public void Not_Remove_Null_Payment()
        {
            var paymentCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, paymentCount);
            vendorInvoice.Payments.Count.Should().Be(paymentCount);
            VendorInvoicePayment nullPayment = null;

            var result = vendorInvoice.RemovePayment(nullPayment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
            vendorInvoice.Payments.Count.Should().Be(paymentCount);
        }

        [Fact]
        public void AddTax()
        {
            var taxLineCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, taxLineCount);
            var taxes = VendorInvoiceTestHelper.CreateTaxes(taxLineCount: taxLineCount, taxAmount: 1);

            foreach (var tax in taxes)
                vendorInvoice.AddTax(tax);

            vendorInvoice.Taxes.Count.Should().Be(taxLineCount + taxes.Count);
        }

        [Fact]
        public void Not_Add_Null_Tax()
        {
            var taxLineCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, taxLineCount);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
            VendorInvoiceTax nullTax = null;

            var result = vendorInvoice.AddTax(nullTax);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);

        }

        [Fact]
        public void RemoveTax()
        {
            var taxLineCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, taxLineCount);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
            var taxToRemove = vendorInvoice.Taxes[1];

            vendorInvoice.RemoveTax(taxToRemove);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount - 1);
        }

        [Fact]
        public void Not_Remove_Null_Tax()
        {
            var taxLineCount = 5;
            var vendor = VendorTestHelper.CreateVendor();
            var vendorInvoice = VendorInvoiceTestHelper.CreateVendorInvoice(vendor, taxLineCount);
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
            VendorInvoiceTax nullTax = null;

            var result = vendorInvoice.RemoveTax(nullTax);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
            vendorInvoice.Taxes.Count.Should().Be(taxLineCount);
        }

        [Fact]
        public void SetVendor()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendorOne);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var vendorTwo = Vendor.Create("Vendor Two", "V@", VendorRole.PartsSupplier).Value;
            vendorInvoice.SetVendor(vendorTwo);

            vendorInvoice.Vendor.Should().Be(vendorTwo);
        }

        [Fact]
        public void SetVendorInvoiceStatus()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;
            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);

            vendorInvoice.SetStatus(VendorInvoiceStatus.Reconciled);

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Reconciled);
        }

        [Fact]
        public void SetInvoiceNumber()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var invoiceNumber = "001";
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var invoiceNumber = "001";
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                invoiceNumber: invoiceNumber,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.DocumentType.Should().Be(VendorInvoiceDocumentType.Unknown);
            var newInvoiceNumber = VendorInvoiceDocumentType.Invoice;
            var resultOrError = vendorInvoice.SetDocumentType(newInvoiceNumber);

            resultOrError.Value.Should().Be(newInvoiceNumber);
        }

        [Fact]
        public void SetTotal()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers)
                .Value;

            var resultOrError = vendorInvoice.SetStatus(invalidStatus);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Invoice_Number()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

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

            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers).Value;

            vendorInvoice.DocumentType.Should().Be(VendorInvoiceDocumentType.Unknown);
            var resultOrError = vendorInvoice.SetDocumentType(invalidDocumentType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Set_Null_Invoice_Number()
        {
            string nullInvoiceNumber = null;
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
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
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var invoiceNumber = "05";
            var invoiceNumbers = new List<int>() { 1, 2, 3, 4 };
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbers(vendor, invoiceNumbers);

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

        [Fact]
        public void UpdateCollections_Can_Add_LineItems()
        {
            var vendorInvoice = VendorInvoiceTestHelper.GenerateVendorInvoices(count: 1)[0];
            vendorInvoice.LineItems.Count.Should().BeGreaterThan(0);
            var lineItemsCount = vendorInvoice.LineItems.Count;

            //Add some LineItems
            var lineItems = new VendorInvoiceLineItemFaker(generateId: true).Generate(3);

            foreach (var lineItem in lineItems)
                vendorInvoice.AddLineItem(lineItem);

            var result = vendorInvoice.UpdateCollections(
                new VendorInvoiceCollections(vendorInvoice.LineItems, vendorInvoice.Payments, vendorInvoice.Taxes));

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.LineItems.Count.Should().Be(lineItemsCount + 3);
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
