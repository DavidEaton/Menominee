using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceTaxShould
    {
        [Fact]
        public void Create_VendorInvoiceTax()
        {
            // Arrange
            var salesTax = VendorInvoiceTestHelper.CreateSalesTax();

            // Act
            var vendorInvoiceTaxOrError = VendorInvoiceTax.Create(salesTax, 1);

            // Assert
            vendorInvoiceTaxOrError.IsFailure.Should().BeFalse();
            vendorInvoiceTaxOrError.Should().NotBeNull();
            vendorInvoiceTaxOrError.Value.Should().BeOfType<VendorInvoiceTax>();
        }

        [Fact]
        public void Not_Create_VendorInvoiceTax_With_Null_SalesTax()
        {
            SalesTax nullSalesTax = null;

            var vendorInvoiceTaxOrError = VendorInvoiceTax.Create(nullSalesTax, 1);

            vendorInvoiceTaxOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoiceTax_With_Invalid_TaxId(long invalidTaxId)
        {
            var salesTax = VendorInvoiceTestHelper.CreateSalesTax();

            var vendorInvoiceTaxOrError = VendorInvoiceTax.Create(salesTax, (int)invalidTaxId);

            vendorInvoiceTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetSalesTax()
        {
            var vendorInvoiceTax = VendorInvoiceTax.Create(VendorInvoiceTestHelper.CreateSalesTax(), 1).Value;

            var salesTax = VendorInvoiceTestHelper.CreateSalesTax(1);
            vendorInvoiceTax.SetSalesTax(salesTax);

            vendorInvoiceTax.SalesTax.Should().Be(salesTax);
        }

        [Fact]
        public void Not_Set_Null_SalesTax()
        {
            var vendorInvoiceTax = VendorInvoiceTax.Create(VendorInvoiceTestHelper.CreateSalesTax(), 1).Value;

            SalesTax nullSalesTax = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoiceTax.SetSalesTax(nullSalesTax));
        }

        [Fact]
        public void SetTaxId()
        {
            var vendorInvoiceTax = VendorInvoiceTax.Create(VendorInvoiceTestHelper.CreateSalesTax(), 1).Value;

            int taxId = 2;
            vendorInvoiceTax.SetTaxId(taxId);

            vendorInvoiceTax.TaxId.Should().Be(taxId);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_TaxId(long invalidTaxId)
        {
            var vendorInvoiceTax = VendorInvoiceTax.Create(VendorInvoiceTestHelper.CreateSalesTax(), 1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoiceTax.SetTaxId((int)invalidTaxId));
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { (long)int.MaxValue + 1 };
                    yield return new object[] { (long)int.MinValue - 1 };
                }
            }
        }
    }
}
