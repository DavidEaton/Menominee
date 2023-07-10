using Bogus;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using System;
using System.Collections.Generic;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderPurchaseShould
    {
        private readonly Faker faker;

        public RepairOrderPurchaseShould()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_RepairOrderPurchase()
        {
            var vendor = new VendorFaker(true).Generate();
            var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
            var pONumber = $"PO-{faker.Finance.Account(10)}";
            var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
            var partNumberFormat = "####-###-####";
            var vendorPartNumber = faker.Random.Replace(partNumberFormat);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, vendorInvoiceNumber, vendorPartNumber);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<RepairOrderPurchase>();
            result.Value.Vendor.Should().Be(vendor);
            result.Value.PurchaseDate.Should().Be(purchaseDate);
            result.Value.PONumber.Should().Be(pONumber);
            result.Value.VendorInvoiceNumber.Should().Be(vendorInvoiceNumber);
            result.Value.VendorPartNumber.Should().Be(vendorPartNumber);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Null_Vendor()
        {
            Vendor vendor = null;
            var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
            var pONumber = $"PO-{faker.Finance.Account(10)}";
            var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
            var partNumberFormat = "####-###-####";
            var vendorPartNumber = faker.Random.Replace(partNumberFormat);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, vendorInvoiceNumber, vendorPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_PurchaseDate()
        {
            var vendor = new VendorFaker(true).Generate();
            var purchaseDate = DateTime.Today.AddDays(1);
            var pONumber = $"PO-{faker.Finance.Account(10)}";
            var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
            var partNumberFormat = "####-###-####";
            var vendorPartNumber = faker.Random.Replace(partNumberFormat);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, vendorInvoiceNumber, vendorPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.DateInvalidMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_PONumber(int length)
        {
            var vendor = new VendorFaker(true).Generate();
            var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
            var invaldPONumber = faker.Finance.Account(length);
            var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
            var partNumberFormat = "####-###-####";
            var vendorPartNumber = faker.Random.Replace(partNumberFormat);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, invaldPONumber, vendorInvoiceNumber, vendorPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_VendorInvoiceNumber(int length)
        {
            var vendor = new VendorFaker(true).Generate();
            var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
            var pONumber = $"PO-{faker.Finance.Account(10)}";
            var invaldVendorInvoiceNumber = faker.Finance.Account(length);
            var partNumberFormat = "####-###-####";
            var vendorPartNumber = faker.Random.Replace(partNumberFormat);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, invaldVendorInvoiceNumber, vendorPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_VendorPartNumber(int length)
        {
            var vendor = new VendorFaker(true).Generate();
            var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
            var pONumber = $"PO-{faker.Finance.Account(10)}";
            var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
            var invaldVendorPartNumber = faker.Finance.Account(length);

            var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, vendorInvoiceNumber, invaldVendorPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        [Fact]
        public void SetVendor()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var newVendor = new VendorFaker(true).Generate();
            purchase.Vendor.Should().NotBe(newVendor);

            var result = purchase.SetVendor(newVendor);

            result.IsSuccess.Should().BeTrue();
            purchase.Vendor.Should().Be(newVendor);
        }

        [Fact]
        public void Return_Failure_On_SetVendor_With_Null_Vendor()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            Vendor newVendor = null;
            purchase.Vendor.Should().NotBe(newVendor);

            var result = purchase.SetVendor(newVendor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.RequiredMessage);
        }

        [Fact]
        public void SetPurchaseDate()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var newPurchaseDate = DateTime.Today.AddDays(-1);
            purchase.PurchaseDate.Should().NotBe(newPurchaseDate);

            var result = purchase.SetPurchaseDate(newPurchaseDate);

            result.IsSuccess.Should().BeTrue();
            purchase.PurchaseDate.Should().Be(newPurchaseDate);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_PurchaseDate()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var invalidPurchaseDate = DateTime.Today.AddDays(1);
            purchase.PurchaseDate.Should().NotBe(invalidPurchaseDate);

            var result = purchase.SetPurchaseDate(invalidPurchaseDate);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.DateInvalidMessage);
        }

        [Fact]
        public void SetPONumber()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var newPONumber = $"PO-{faker.Finance.Account(10)}";
            purchase.PONumber.Should().NotBe(newPONumber);

            var result = purchase.SetPONumber(newPONumber);

            result.IsSuccess.Should().BeTrue();
            purchase.PONumber.Should().Be(newPONumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_PONumber(int length)
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var invaldPONumber = faker.Finance.Account(length);
            purchase.PONumber.Should().NotBe(invaldPONumber);

            var result = purchase.SetPONumber(invaldPONumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        [Fact]
        public void SetVendorInvoiceNumber()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var newVendorInvoiceNumber = $"PO-{faker.Finance.Account(10)}";
            purchase.VendorInvoiceNumber.Should().NotBe(newVendorInvoiceNumber);

            var result = purchase.SetVendorInvoiceNumber(newVendorInvoiceNumber);

            result.IsSuccess.Should().BeTrue();
            purchase.VendorInvoiceNumber.Should().Be(newVendorInvoiceNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_InvoiceNumber(int length)
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var invaldInvoiceNumber = faker.Finance.Account(length);
            purchase.VendorInvoiceNumber.Should().NotBe(invaldInvoiceNumber);

            var result = purchase.SetVendorInvoiceNumber(invaldInvoiceNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        [Fact]
        public void SetVendorPartNumber()
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var newVendorPartNumber = $"PO-{faker.Finance.Account(10)}";
            purchase.VendorPartNumber.Should().NotBe(newVendorPartNumber);

            var result = purchase.SetVendorPartNumber(newVendorPartNumber);

            result.IsSuccess.Should().BeTrue();
            purchase.VendorPartNumber.Should().Be(newVendorPartNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_PartNumber(int length)
        {
            var purchase = new RepairOrderPurchaseFaker(true).Generate();
            var invaldPartNumber = faker.Finance.Account(length);
            purchase.VendorPartNumber.Should().NotBe(invaldPartNumber);

            var result = purchase.SetVendorPartNumber(invaldPartNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPurchase.InvalidLengthMessage);
        }

        public class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { null };
                    yield return new object[] { RepairOrderPurchase.MinimumLength - 1 };
                    yield return new object[] { RepairOrderPurchase.MaximumLength + 1 };
                }
            }
        }

    }
}
