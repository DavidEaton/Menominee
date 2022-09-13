using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.UnitTests.EntityTests.VendorInvoiceTestHelper;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class SalesTaxShould
    {
        [Fact]
        public void Create_SalesTax()
        {
            // Arrange
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;
            bool? isAppliedByDefault = true;
            bool? isTaxable = true;
            //List<ExciseFee> exciseFees = VendorInvoiceTestHelper.CreateExciseFees();
            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            // TODO: VendorInvoiceTestHelper.CreateExciseFees() creates and returns an entity list
            // with all Id == 0. That breaks identity comaprisons like
            // if (!ExciseFees.Any(x => x.Id == fee.Id))... inside our
            // domain class SalesTax.SetExciseFees creation/validation
            // MUST TEST COLLECTIONS WITH INTEGRATION, NOT UNIT TESTS

            // TODO: INTEGRATION TESTS:
            // Create_SalesTax_With_Optional_ExciseFees()
            // Not_Create_SalesTax_With_Duplicate_ExciseFees()
            // ??? OR
            // ExciseFeeShould.Not_Create_Duplicate_ExciseFee()


            // Act
            var salesTaxOrError = SalesTax.Create(description, taxType, order, taxIdNumber, partTaxRate, laborTaxRate, isAppliedByDefault: isAppliedByDefault, isTaxable: isTaxable);

            // Assert
            salesTaxOrError.Value.Should().BeOfType<SalesTax>();
            salesTaxOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Null_Description()
        {
            string nullDescription = null;

            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(nullDescription, taxType, order, taxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Invalid_Description()
        {
            var invalidDescription = Utilities.RandomCharacters((int)SalesTax.DescriptionMaximumLength + 1);

            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(invalidDescription, taxType, order, taxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Invalid_TaxType()
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);

            var invalidSalesTaxType = (SalesTaxType)(-1);

            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(description, invalidSalesTaxType, order, taxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Invalid_Order()
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;

            var invalidOrder = (int)SalesTax.MinimumValue - 1;

            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(description, taxType, invalidOrder, taxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Null_TaxIdNumber()
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 1;

            string invalidTaxIdNumber = null;

            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(description, taxType, order, invalidTaxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_SalesTax_With_Invalid_TaxIdNumber(int length)
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 1;

            var invalidTaxIdNumber = Utilities.RandomCharacters(length);

            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(description, taxType, order, invalidTaxIdNumber, partTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Invalid_PartTaxRate()
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 1;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);

            var invalidPartTaxRate = SalesTax.MinimumValue - .1;

            var laborTaxRate = SalesTax.MinimumValue + .25;

            var salesTaxOrError = SalesTax.Create(description, taxType, order, taxIdNumber, invalidPartTaxRate, laborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_SalesTax_With_Invalid_LaborTaxRate()
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 1;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;

            var invalidLaborTaxRate = SalesTax.MinimumValue - .25;

            var salesTaxOrError = SalesTax.Create(description, taxType, order, taxIdNumber, partTaxRate, invalidLaborTaxRate);

            salesTaxOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetDescription()
        {
            var salesTax = CreateSalesTax();

            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 150);
            salesTax.SetDescription(description);

            salesTax.Description.Should().Be(description);
        }

        [Fact]
        public void Not_Set_Null_Description()
        {
            var salesTax = CreateSalesTax();

            var resultOrError =  salesTax.SetDescription(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Description()
        {
            var salesTax = CreateSalesTax();
            string invalidDescription = Utilities.RandomCharacters(SalesTax.DescriptionMaximumLength + 1);

            var resultOrError =  salesTax.SetDescription(invalidDescription);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetTaxType()
        {
            var salesTax = CreateSalesTax();

            salesTax.TaxType.Should().Be(SalesTaxType.Normal);
            salesTax.SetTaxType(SalesTaxType.GST);

            salesTax.TaxType.Should().Be(SalesTaxType.GST);
        }

        [Fact]
        public void Not_Set_Invalid_TaxType()
        {
            var salesTax = CreateSalesTax();
            var invalidSalesTaxType = (SalesTaxType)(-1);

            var resultOrError =  salesTax.SetTaxType(invalidSalesTaxType);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetOrder()
        {
            var salesTax = CreateSalesTax();

            salesTax.Order.Should().Be((int)SalesTax.MinimumValue + 10);
            salesTax.SetOrder((int)SalesTax.MinimumValue + 1);

            salesTax.Order.Should().Be((int)SalesTax.MinimumValue + 1);
        }

        [Fact]
        public void Not_Set_Invalid_Order()
        {
            var salesTax = CreateSalesTax();
            salesTax.Order.Should().Be((int)SalesTax.MinimumValue + 10);

            var resultOrError =  salesTax.SetOrder((int)SalesTax.MinimumValue - 1);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetTaxIdNumber()
        {
            var salesTax = CreateSalesTax();

            salesTax.TaxIdNumber.Length.Should().Be(Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11).Length);
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 1);
            salesTax.SetTaxIdNumber(taxIdNumber);

            salesTax.TaxIdNumber.Should().Be(taxIdNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_TaxIdNumber(int length)
        {
            var salesTax = CreateSalesTax();
            salesTax.TaxIdNumber.Length.Should().Be(Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11).Length);
            var invalidTaxIdNumber = Utilities.RandomCharacters(length);

            var resultOrError =  salesTax.SetTaxIdNumber(invalidTaxIdNumber);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_TaxIdNumber()
        {
            var salesTax = CreateSalesTax();
            salesTax.TaxIdNumber.Length.Should().Be(Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11).Length);

            var resultOrError =  salesTax.SetTaxIdNumber(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetPartTaxRate()
        {
            var salesTax = CreateSalesTax();

            salesTax.PartTaxRate.Should().Be(SalesTax.MinimumValue + .1);
            salesTax.SetPartTaxRate(SalesTax.MinimumValue + .2);

            salesTax.PartTaxRate.Should().Be(SalesTax.MinimumValue + .2);
        }

        [Fact]
        public void Not_Set_Invalid_PartTaxRate()
        {
            var salesTax = CreateSalesTax();
            salesTax.PartTaxRate.Should().Be(SalesTax.MinimumValue + .1);
            var invalidPartTaxRate = SalesTax.MinimumValue - .1;

            var resultOrError =  salesTax.SetPartTaxRate(invalidPartTaxRate);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetLaborTaxRate()
        {
            var salesTax = CreateSalesTax();

            salesTax.LaborTaxRate.Should().Be(SalesTax.MinimumValue + .25);
            salesTax.SetLaborTaxRate(SalesTax.MinimumValue + .5);

            salesTax.LaborTaxRate.Should().Be(SalesTax.MinimumValue + .5);
        }

        [Fact]
        public void Not_Set_Invalid_LaborTaxRate()
        {
            var salesTax = CreateSalesTax();
            salesTax.LaborTaxRate.Should().Be(SalesTax.MinimumValue + .25);
            var invalidLaborTaxRate = SalesTax.MinimumValue - .1;

            var resultOrError =  salesTax.SetLaborTaxRate(invalidLaborTaxRate);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetIsAppliedByDefault()
        {
            var salesTax = CreateSalesTax();

            salesTax.IsAppliedByDefault.Should().BeTrue();
            salesTax.SetIsAppliedByDefault(false);

            salesTax.IsAppliedByDefault.Should().BeFalse();
        }

        [Fact]
        public void SetIsTaxable()
        {
            var salesTax = CreateSalesTax();

            salesTax.IsTaxable.Should().BeTrue();
            salesTax.SetIsTaxable(false);

            salesTax.IsTaxable.Should().BeFalse();
        }

        [Fact]
        public void AddExciseFee()
        {
            var salesTax = CreateSalesTax();
            var fee = ExciseFee.Create(
                Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1),
                ExciseFeeType.Flat,
                ExciseFee.MinimumValue + 1.0).Value;

            salesTax.AddExciseFee(fee);

            salesTax.ExciseFees[0].Should().Be(fee);
        }

        [Fact]
        public void RemoveExciseFee()
        {
            var salesTax = CreateSalesTax();
            var fee = ExciseFee.Create(
                Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1),
                ExciseFeeType.Flat,
                ExciseFee.MinimumValue + 1.0).Value;
            salesTax.AddExciseFee(fee);

            salesTax.ExciseFees.Count.Should().Be(1);
            salesTax.RemoveExciseFee(fee);

            salesTax.ExciseFees.Count.Should().Be(0);
        }

        [Fact]
        public void Not_Remove_NulL_ExciseFee()
        {
            var salesTax = CreateSalesTax();
            var fee = ExciseFee.Create(
                Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1),
                ExciseFeeType.Flat,
                ExciseFee.MinimumValue + 1.0).Value;
            salesTax.AddExciseFee(fee);
            salesTax.ExciseFees.Count.Should().Be(1);

            var result = salesTax.RemoveExciseFee(null);

            result.IsFailure.Should().BeTrue();
            salesTax.ExciseFees.Count.Should().Be(1);
        }

        [Fact]
        public void Not_Add_Null_ExciseFee()
        {
            var salesTax = CreateSalesTax();

            var result = salesTax.AddExciseFee(null);

            result.IsFailure.Should().BeTrue();
        }

        public class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { SalesTax.TaxIdNumberMinumumLength - 1 };
                    yield return new object[] { SalesTax.TaxIdNumberMaximumLength + 1 };
                }
            }
        }
    }
}
