using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Shared.Models.Payables.Invoices.LineItems.Items;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Shared.Models.Taxes;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary.Fakers;
using TestingHelperLibrary.Payables;
using Xunit;

namespace Menominee.Tests.Entities
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
            var result = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: "");

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<VendorInvoice>();
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

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                datePosted: datePosted);

            result.IsFailure.Should().BeFalse();
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_Date()
        {
            var date = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: date);

            result.IsFailure.Should().BeFalse();
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<VendorInvoice>();
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
        public void Return_Failure_On_Create_VendorInvoice_With_Null_Vendor()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor: null,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                invalidStatus,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_DocumentType()
        {
            var invalidDocumentType = (VendorInvoiceDocumentType)(-1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                invalidDocumentType,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_Total()
        {
            var invalidTotal = -1;
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                invalidTotal,
                vendorInvoiceNumbers: vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_Date()
        {
            var invalidDate = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: invalidDate);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_DatePosted()
        {
            var date = DateTime.Today;
            var invalidDatePosted = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                date: date,
                datePosted: invalidDatePosted);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Invalid_InvoiceNumber()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: invalidInvoiceNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Create_VendorInvoice_With_Nonunique_Vendor_InvoiceNumber()
        {
            var vendor = Vendor.Create("Vendor One", "V1", VendorRole.PartsSupplier).Value;
            var invoiceNumbers = new List<int>() { 1, 2, 3, 4 };
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbers(vendor, invoiceNumbers);

            var result = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                VendorInvoiceDocumentType.Unknown,
                total: 0,
                vendorInvoiceNumbers: vendorInvoiceNumbers,
                invoiceNumber: $"{vendor.Id}{2}");

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Should().Contain("unique");
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
        public void Return_Failure_On_AddLineItem_With_Null_Input()
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
        public void Return_Failure_On_RemoveLineItem_With_Null_Input()
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
        public void Return_Failure_On_AddPayment_With_Null_Input()
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
        public void Return_Failure_On_RemovePayment_With_Null_Input()
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
        public void Return_Failure_On_AddTax_With_Null_Input()
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
        public void Return_Failure_On_RemoveTax_With_Null_Input()
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
        public void Return_Failure_On_SetVendor_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetVendor(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.RequiredMessage);
        }

        [Fact]
        public void SetStatus()
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
        public void Return_Failure_On_SetStatus_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetStatus((VendorInvoiceStatus)(-1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.RequiredMessage);
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
        public void Return_Failure_On_SetInvoiceNumber_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();
            List<string> vendorInvoiceNumbers = new();
            var result = vendorInvoice.SetInvoiceNumber(null, vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_SetInvoiceNumber_With_Duplicate_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();
            var vendor = new VendorFaker(true).Generate();
            var vendorInvoiceNumbers = VendorInvoiceTestHelper.CreateVendorInvoiceNumbersList(vendor);
            var newInvoiceNumber = vendorInvoiceNumbers[0];

            var result = vendorInvoice.SetInvoiceNumber(newInvoiceNumber, vendorInvoiceNumbers);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.NonuniqueMessage);
        }

        [Fact]
        public void SetDocumentType()
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
        public void Return_Failure_On_SetDocumentType_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetDocumentType((VendorInvoiceDocumentType)(-1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.RequiredMessage);
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
        public void Return_Failure_On_SetTotal_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetTotal(-1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.MinimumValueMessage);
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

            vendorInvoice.Date.Should().BeNull();
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDate(date);

            vendorInvoice.Date.Should().Be(date);
        }

        [Fact]
        public void Return_Failure_On_SetDate_With_Null_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetDate(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.DateInvalidMessage);
        }

        [Fact]
        public void Return_Failure_On_SetDate_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetDate(DateTime.Today.AddDays(1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.DateInvalidMessage);
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
        public void Return_Failure_On_SetDatePosted_With_Null_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetDatePosted(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.DateInvalidMessage);
        }

        [Fact]
        public void Return_Failure_On_SetDatePosted_With_Invalid_Input()
        {
            var vendorInvoice = new VendorInvoiceFaker(true).Generate();

            var result = vendorInvoice.SetDatePosted(DateTime.Today.AddDays(1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(VendorInvoice.DateInvalidMessage);
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
        public void Return_Failure_On_Set_Null_Vendor()
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
        public void Return_Failure_On_Set_Invalid_VendorInvoice_Status()
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
        public void Return_Failure_On_Set_Invalid_Invoice_Number()
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
        public void Return_Failure_On_Set_Invalid_VendorInvoiceDocumentType()
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
        public void Return_Failure_On_Set_Null_Invoice_Number()
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
        public void Return_Failure_On_Set_Invalid_Total()
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
        public void Return_Failure_On_Set_Invalid_Date(DateTime? date)
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
        public void Return_Failure_On_Set_Invalid_DatePosted(DateTime? datePosted)
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
        public void Return_Failure_On_Set_Nonunique_InvoiceNumber()
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
        public void Add_Added_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, taxesCount: taxesCount).Generate();
            var addedTaxes = new VendorInvoiceTaxFaker(generateId: false).Generate(taxesCount);
            var updatedTaxes = vendorInvoice.Taxes.Concat(addedTaxes).ToList();
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(addedTaxes);

            var result = vendorInvoice.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(addedTaxes);
            vendorInvoice.Taxes.Count.Should().Be(taxesCount + taxesCount);
        }

        [Fact]
        public void Update_Edited_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, taxesCount: taxesCount).Generate();
            var originalTaxes = vendorInvoice.Taxes.Select(tax =>
            {
                return new VendorInvoiceTaxToWrite
                {
                    Id = tax.Id,
                    SalesTax = SalesTaxHelper.ConvertToReadDto(tax.SalesTax),
                    Amount = tax.Amount,
                };
            }).ToList();
            var editedTaxes = vendorInvoice.Taxes.Select(tax =>
            {
                return new VendorInvoiceTaxToWrite
                {
                    Id = tax.Id,
                    SalesTax = SalesTaxHelper.ConvertToReadDto(tax.SalesTax),
                    Amount = tax.Amount + 1,
                };
            }).ToList();
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(editedTaxes);
            var updatedTaxes = VendorInvoiceTestHelper.CreateVendorInvoiceTaxes(editedTaxes);

            var result = vendorInvoice.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Taxes.Count.Should().Be(taxesCount);
            vendorInvoice.Taxes.Should().BeEquivalentTo(editedTaxes);
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(originalTaxes);
        }

        [Fact]
        public void Delete_Deleted_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, taxesCount: taxesCount).Generate();
            var originalTaxes = vendorInvoice.Taxes.Select(tax =>
            {
                return new VendorInvoiceTaxToWrite
                {
                    Id = tax.Id,
                    SalesTax = SalesTaxHelper.ConvertToReadDto(tax.SalesTax),
                    Amount = tax.Amount,
                };
            }).ToList();
            var deletedTaxes = vendorInvoice.Taxes.ToList();
            deletedTaxes.RemoveAt(0);
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(deletedTaxes);
            var updatedTaxes = deletedTaxes;

            var result = vendorInvoice.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Taxes.Count.Should().Be(taxesCount - 1);
            vendorInvoice.Taxes.Should().NotBeEquivalentTo(originalTaxes);
        }

        [Fact]
        public void Add_Added_Payments_On_UpdatePayments()
        {
            var paymentsCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, paymentsCount: paymentsCount).Generate();
            var addedPayments = new VendorInvoicePaymentFaker(generateId: false).Generate(paymentsCount);
            var updatedPayments = vendorInvoice.Payments.Concat(addedPayments).ToList();
            vendorInvoice.Payments.Should().NotBeEquivalentTo(addedPayments);

            var result = vendorInvoice.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Payments.Should().NotBeEquivalentTo(addedPayments);
            vendorInvoice.Payments.Count.Should().Be(paymentsCount + paymentsCount);
        }

        [Fact]
        public void Update_Edited_Payments_On_UpdatePayments()
        {
            var paymentsCount = 1;
            var vendorInvoice = new VendorInvoiceFaker(true, paymentsCount: paymentsCount).Generate();
            var originalPayments = vendorInvoice.Payments.Select(payment =>
            {
                return new VendorInvoicePaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertToReadDto(payment.PaymentMethod),
                    Amount = payment.Amount,
                };
            }).ToList();
            var editedPayments = vendorInvoice.Payments.Select(payment =>
            {
                return new VendorInvoicePaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertToReadDto(payment.PaymentMethod),
                    Amount = payment.Amount + 1,
                };
            }).ToList();
            vendorInvoice.Payments.Should().NotBeEquivalentTo(editedPayments);
            var updatedPayments = VendorInvoiceTestHelper.CreateVendorInvoicePayments(editedPayments);

            var result = vendorInvoice.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Payments.Count.Should().Be(paymentsCount);
            foreach (var payment in vendorInvoice.Payments)
            {
                var paymentFromCaller = editedPayments.Single(callerPayment => callerPayment.Id == payment.Id);
                paymentFromCaller.Should().NotBeNull();
                paymentFromCaller.Amount.Should().Be(payment.Amount);
                paymentFromCaller.PaymentMethod.Id.Should().Be(payment.PaymentMethod.Id);
            }
            vendorInvoice.Payments.Should().NotBeEquivalentTo(originalPayments);
        }

        [Fact]
        public void Delete_Deleted_Payments_On_UpdatePayments()
        {
            var paymentsCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, paymentsCount: paymentsCount).Generate();
            var originalPayments = vendorInvoice.Payments.Select(payment =>
            {
                return new VendorInvoicePaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertToReadDto(payment.PaymentMethod),
                    Amount = payment.Amount,
                };
            }).ToList();
            var deletedPayments = vendorInvoice.Payments.ToList();
            deletedPayments.RemoveAt(0);
            vendorInvoice.Payments.Should().NotBeEquivalentTo(deletedPayments);
            var updatedPayments = deletedPayments;

            var result = vendorInvoice.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.Payments.Count.Should().Be(paymentsCount - 1);
            vendorInvoice.Payments.Should().NotBeEquivalentTo(originalPayments);
        }

        [Fact]
        public void Add_Added_LineItems_On_UpdateLineItems()
        {
            var lineItemsCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, lineItemsCount: lineItemsCount).Generate();
            var addedLineItems = new VendorInvoiceLineItemFaker(generateId: false).Generate(lineItemsCount);
            var updatedLineItems = vendorInvoice.LineItems.Concat(addedLineItems).ToList();
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(addedLineItems);

            var result = vendorInvoice.UpdateLineItems(updatedLineItems);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(addedLineItems);
            vendorInvoice.LineItems.Count.Should().Be(lineItemsCount + lineItemsCount);
        }

        [Fact]
        public void Update_Edited_LineItems_On_UpdateLineItems()
        {
            var lineItemsCount = 1;
            var vendorInvoice = new VendorInvoiceFaker(true, lineItemsCount: lineItemsCount).Generate();
            var originalLineItems = vendorInvoice.LineItems.Select(lineItem =>
            {
                return new VendorInvoiceLineItemToWrite
                {
                    Id = lineItem.Id,
                    Type = lineItem.Type,
                    Item = VendorInvoiceItemHelper.ConvertToReadDto(lineItem.Item),
                    Quantity = lineItem.Quantity,
                    Cost = lineItem.Cost,
                    Core = lineItem.Core,
                    PONumber = lineItem.PONumber,
                    TransactionDate = lineItem.TransactionDate
                };
            }).ToList();
            var editedLineItems = vendorInvoice.LineItems.Select(lineItem =>
            {
                return new VendorInvoiceLineItemToWrite
                {
                    Id = lineItem.Id,
                    Type = lineItem.Type,
                    Item = VendorInvoiceItemHelper.ConvertToReadDto(lineItem.Item),
                    Quantity = lineItem.Quantity + 1,
                    Cost = lineItem.Cost + 1,
                    Core = lineItem.Core + 1,
                    PONumber = lineItem.PONumber,
                    TransactionDate = lineItem.TransactionDate
                };
            }).ToList();
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(editedLineItems);
            var updatedLineItems = VendorInvoiceTestHelper.CreateVendorInvoiceLineItems(editedLineItems);

            var result = vendorInvoice.UpdateLineItems(updatedLineItems);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.LineItems.Count.Should().Be(lineItemsCount);
            foreach (var lineItem in vendorInvoice.LineItems)
            {
                var lineItemFromCaller = editedLineItems.Single(callerLineItem => callerLineItem.Id == lineItem.Id);
                lineItemFromCaller.Should().NotBeNull();
                lineItemFromCaller.Item.Description.Should().Be(lineItem.Item.Description);
                lineItemFromCaller.Item.PartNumber.Should().Be(lineItem.Item.PartNumber);
                lineItemFromCaller.Cost.Should().Be(lineItem.Cost);
                lineItemFromCaller.Core.Should().Be(lineItem.Core);
                lineItemFromCaller.Quantity.Should().Be(lineItem.Quantity);
                lineItemFromCaller.PONumber.Should().Be(lineItem.PONumber);
                lineItemFromCaller.Type.Should().Be(lineItem.Type);
            }
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(originalLineItems);
        }

        [Fact]
        public void Delete_Deleted_LineItems_On_UpdateLineItems()
        {
            var lineItemsCount = 3;
            var vendorInvoice = new VendorInvoiceFaker(true, lineItemsCount: lineItemsCount).Generate();
            var originalLineItems = vendorInvoice.LineItems.Select(lineItem =>
            {
                return new VendorInvoiceLineItemToWrite
                {
                    Id = lineItem.Id,
                    Type = lineItem.Type,
                    Item = VendorInvoiceItemHelper.ConvertToReadDto(lineItem.Item),
                    Quantity = lineItem.Quantity + 1,
                    Cost = lineItem.Cost + 1,
                    Core = lineItem.Core + 1,
                    PONumber = lineItem.PONumber,
                    TransactionDate = lineItem.TransactionDate
                };
            }).ToList();
            var deletedLineItems = vendorInvoice.LineItems.ToList();
            deletedLineItems.RemoveAt(0);
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(deletedLineItems);
            var updatedLineItems = deletedLineItems;

            var result = vendorInvoice.UpdateLineItems(updatedLineItems);

            result.IsSuccess.Should().BeTrue();
            vendorInvoice.LineItems.Count.Should().Be(lineItemsCount - 1);
            vendorInvoice.LineItems.Should().NotBeEquivalentTo(originalLineItems);
        }

        public class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { null };
                    yield return new object[] { GetFutureDate() };
                }
            }

            private static DateTime? GetFutureDate()
            {
                return DateTime.Today.AddDays(1);
            }
        }
    }
}
