using Bogus;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderLineItemShould
    {
        [Fact]
        public void Create_RepairOrderLineItem()
        {
            var faker = new Faker();
            var item = new RepairOrderItemFaker().Generate();
            var saleType = faker.PickRandom<SaleType>();
            var isCounterSale = faker.Random.Bool();
            var isDeclined = faker.Random.Bool();
            var quantitySold = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var sellingPrice = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var laborAmount = LaborAmount.Create(type, amount).Value;
            var cost = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var discountAmount = DiscountAmount.Create(ItemDiscountType.Predefined, amount).Value;

            var result = RepairOrderLineItem.Create(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<RepairOrderLineItem>();
        }

        [Fact]
        public void Return_Failure_When_Item_Is_Null()
        {
            var faker = new Faker();
            var item = (RepairOrderItem)null;
            var saleType = faker.PickRandom<SaleType>();
            var isDeclined = faker.Random.Bool();
            var isCounterSale = faker.Random.Bool();
            var quantitySold = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var sellingPrice = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var laborAmount = LaborAmount.Create(type, amount).Value;
            var cost = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var discountAmount = DiscountAmount.Create(ItemDiscountType.Predefined, amount).Value;

            var result = RepairOrderLineItem.Create(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_When_SaleType_Is_Invalid()
        {
            var faker = new Faker();
            var item = new RepairOrderItemFaker().Generate();
            var invalidSaleType = (SaleType)(-1);
            var isDeclined = faker.Random.Bool();
            var isCounterSale = faker.Random.Bool();
            var quantitySold = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var sellingPrice = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var laborAmount = LaborAmount.Create(type, amount).Value;
            var cost = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var discountAmount = DiscountAmount.Create(ItemDiscountType.Predefined, amount).Value;

            var result = RepairOrderLineItem.Create(item, invalidSaleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void SetItem()
        {
            var faker = new Faker();
            var item = new RepairOrderItemFaker().Generate();
            var saleType = faker.PickRandom<SaleType>();
            var isDeclined = faker.Random.Bool();
            var isCounterSale = faker.Random.Bool();
            var quantitySold = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var sellingPrice = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var laborAmount = LaborAmount.Create(type, amount).Value;
            var cost = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var discountAmount = DiscountAmount.Create(ItemDiscountType.Predefined, amount).Value;
            var repairOrderLineItem = RepairOrderLineItem.Create(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount).Value;
            var newItem = new RepairOrderItemFaker().Generate();
            repairOrderLineItem.Item.Should().NotBe(newItem);
            var result = repairOrderLineItem.SetItem(newItem);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newItem);
            repairOrderLineItem.Item.Should().Be(newItem);
        }

        [Fact]
        public void Return_Failure_On_SetItem_When_Item_Is_Null()
        {
            var faker = new Faker();
            var item = new RepairOrderItemFaker().Generate();
            var saleType = faker.PickRandom<SaleType>();
            var isDeclined = faker.Random.Bool();
            var isCounterSale = faker.Random.Bool();
            var quantitySold = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var sellingPrice = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var laborAmount = LaborAmount.Create(type, amount).Value;
            var cost = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var discountAmount = DiscountAmount.Create(ItemDiscountType.Predefined, amount).Value;
            var repairOrderLineItem = RepairOrderLineItem.Create(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount).Value;
            var newItem = (RepairOrderItem)null;

            var result = repairOrderLineItem.SetItem(newItem);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void SetSaleType()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var newSaleType = faker.PickRandom<SaleType>();
            while (lineItem.SaleType == newSaleType)
                newSaleType = faker.PickRandom<SaleType>();
            lineItem.SaleType.Should().NotBe(newSaleType);

            var result = lineItem.SetSaleType(newSaleType);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newSaleType);
            lineItem.SaleType.Should().Be(newSaleType);
        }

        [Fact]
        public void Return_Failure_On_SetSaleType_When_SaleType_Is_Invalid()
        {
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var invalidSaleType = (SaleType)(-1);

            var result = lineItem.SetSaleType(invalidSaleType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void SetIsDeclined()
        {
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var isDeclined = new Faker().Random.Bool();
            var result = lineItem.SetIsDeclined(isDeclined);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(isDeclined);

            result = lineItem.SetIsDeclined(!isDeclined);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(!isDeclined);
        }

        [Fact]
        public void SetIsCounterSale()
        {
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var isCounterSale = new Faker().Random.Bool();
            var result = lineItem.SetIsCounterSale(isCounterSale);
            result.IsSuccess.Should().BeTrue();
            lineItem.IsCounterSale.Should().Be(isCounterSale);

            result = lineItem.SetIsCounterSale(!isCounterSale);

            result.IsSuccess.Should().BeTrue();
            lineItem.IsCounterSale.Should().Be(!isCounterSale);
        }

        [Fact]
        public void SetQuantitySold()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var newQuantitySold = faker.Random.Double(1, 1000);
            while (Math.Abs(lineItem.QuantitySold - newQuantitySold) < 0.01)
                newQuantitySold = faker.Random.Double(1, 1000);
            lineItem.QuantitySold.Should().NotBe(newQuantitySold);

            var result = lineItem.SetQuantitySold(newQuantitySold);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newQuantitySold);
            lineItem.QuantitySold.Should().Be(newQuantitySold);
        }

        [Fact]
        public void SetSellingPrice()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var newSellingPrice = faker.Random.Double(1, 1000);
            while (Math.Abs(lineItem.SellingPrice - newSellingPrice) < 0.01)
                newSellingPrice = faker.Random.Double(1, 1000);
            lineItem.SellingPrice.Should().NotBe(newSellingPrice);

            var result = lineItem.SetSellingPrice(newSellingPrice);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newSellingPrice);
            lineItem.SellingPrice.Should().Be(newSellingPrice);
        }

        [Fact]
        public void SetLaborAmount()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var type = faker.PickRandom<ItemLaborType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var newLaborAmount = LaborAmount.Create(type, amount).Value;
            while (lineItem.LaborAmount == newLaborAmount)
            {
                type = faker.PickRandom<ItemLaborType>();
                amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
                newLaborAmount = LaborAmount.Create(type, amount).Value;
            }
            lineItem.LaborAmount.Should().NotBe(newLaborAmount);

            var result = lineItem.SetLaborAmount(newLaborAmount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newLaborAmount);
            lineItem.LaborAmount.Should().Be(newLaborAmount);
        }

        [Fact]
        public void Return_Failure_On_SetLaborAmount_When_LaborAmount_Is_Null()
        {
            var lineItem = new RepairOrderLineItemFaker().Generate();
            LaborAmount laborAmount = null;

            var result = lineItem.SetLaborAmount(laborAmount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void SetCost()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var newCost = faker.Random.Double(1, 1000);
            while (Math.Abs(lineItem.Cost - newCost) < 0.01)
            {
                newCost = faker.Random.Double(1, 1000);
            }

            var result = lineItem.SetCost(newCost);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeApproximately(newCost, 0.01);
            lineItem.Cost.Should().BeApproximately(newCost, 0.01);
        }

        [Fact]
        public void SetCore()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var newCore = faker.Random.Double(1, 1000);
            while (Math.Abs(lineItem.Core - newCore) < 0.001)
                newCore = faker.Random.Double(1, 1000);
            lineItem.Core.Should().NotBe(newCore);

            var result = lineItem.SetCore(newCore);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newCore);
            lineItem.Core.Should().Be(newCore);
        }

        [Fact]
        public void SetDiscountAmount()
        {
            var faker = new Faker();
            var lineItem = new RepairOrderLineItemFaker().Generate();
            var type = faker.PickRandom<ItemDiscountType>();
            var amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
            var newDiscountAmount = DiscountAmount.Create(type, amount).Value;
            while (lineItem.DiscountAmount == newDiscountAmount)
            {
                type = faker.PickRandom<ItemDiscountType>();
                amount = (double)Math.Round(faker.Random.Decimal(1, 99), 2);
                newDiscountAmount = DiscountAmount.Create(type, amount).Value;
            }
            lineItem.DiscountAmount.Should().NotBe(newDiscountAmount);

            var result = lineItem.SetDiscountAmount(newDiscountAmount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newDiscountAmount);
            lineItem.DiscountAmount.Should().Be(newDiscountAmount);
        }

        [Fact]
        public void Return_Failure_On_SetDiscountAmount_When_DiscountAmount_Is_Null()
        {
            var lineItem = new RepairOrderLineItemFaker().Generate();
            DiscountAmount discountAmount = null;

            var result = lineItem.SetDiscountAmount(discountAmount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void AddSerialNumber()
        {
            var serialNumbersCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, serialNumbersCount).Generate();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount);
            var serialNumber = new RepairOrderSerialNumberFaker().Generate();

            var result = lineItem.AddSerialNumber(serialNumber);

            result.IsSuccess.Should().BeTrue();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount + 1);
            lineItem.SerialNumbers.Should().Contain(serialNumber);
        }

        [Fact]
        public void Return_Failure_On_AddSerialNumber_When_SerialNumber_Is_Invalid()
        {
            var serialNumbersCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, serialNumbersCount).Generate();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount);

            var result = lineItem.AddSerialNumber(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void RemoveSerialNumber()
        {
            var serialNumbersCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, serialNumbersCount).Generate();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount);
            var toRemove = lineItem.SerialNumbers[0];

            var result = lineItem.RemoveSerialNumber(toRemove);

            result.IsSuccess.Should().BeTrue();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount - 1);
            lineItem.SerialNumbers.Should().NotContain(toRemove);
        }

        [Fact]
        public void Return_Failure_On_RemoveSerialNumber_When_SerialNumber_Is_Invalid()
        {
            var serialNumbersCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, serialNumbersCount).Generate();
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount);

            var result = lineItem.RemoveSerialNumber(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
            lineItem.SerialNumbers.Count.Should().Be(serialNumbersCount);
        }

        [Fact]
        public void AddWarranty()
        {
            var warrantiesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, warrantiesCount).Generate();
            lineItem.Warranties.Count.Should().Be(warrantiesCount);
            var warranty = new RepairOrderWarrantyFaker().Generate();

            var result = lineItem.AddWarranty(warranty);

            result.IsSuccess.Should().BeTrue();
            lineItem.Warranties.Count.Should().Be(warrantiesCount + 1);
            lineItem.Warranties.Should().Contain(warranty);
        }

        [Fact]
        public void Return_Failure__On_AddWarranty_When_Warranty_Is_Invalid()
        {
            var warrantiesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, warrantiesCount).Generate();
            lineItem.Warranties.Count.Should().Be(warrantiesCount);

            var result = lineItem.AddWarranty(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void RemoveWarranty()
        {
            var warrantiesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, warrantiesCount).Generate();
            lineItem.Warranties.Count.Should().Be(warrantiesCount);
            var toRemove = lineItem.Warranties[0];

            var result = lineItem.RemoveWarranty(toRemove);

            result.IsSuccess.Should().BeTrue();
            lineItem.Warranties.Count.Should().Be(warrantiesCount - 1);
            lineItem.Warranties.Should().NotContain(toRemove);
        }

        [Fact]
        public void Return_Failure_On_RemoveWarranty_When_Warranty_Is_Invalid()
        {
            var warrantiesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, warrantiesCount).Generate();
            lineItem.Warranties.Count.Should().Be(warrantiesCount);

            var result = lineItem.RemoveWarranty(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
            lineItem.Warranties.Count.Should().Be(warrantiesCount);
        }

        [Fact]
        public void AddTax()
        {
            var taxesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, taxesCount).Generate();
            lineItem.Taxes.Count.Should().Be(taxesCount);
            var tax = new RepairOrderItemTaxFaker().Generate();

            var result = lineItem.AddTax(tax);

            result.IsSuccess.Should().BeTrue();
            lineItem.Taxes.Count.Should().Be(taxesCount + 1);
            lineItem.Taxes.Should().Contain(tax);
        }

        [Fact]
        public void Return_Failure_On_AddTax_When_Tax_Is_Invalid()
        {
            var taxesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, taxesCount).Generate();
            lineItem.Taxes.Count.Should().Be(taxesCount);

            var result = lineItem.AddTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void RemoveTax()
        {
            var taxesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, taxesCount).Generate();
            lineItem.Taxes.Count.Should().Be(taxesCount);
            var toRemove = lineItem.Taxes[0];

            var result = lineItem.RemoveTax(toRemove);

            result.IsSuccess.Should().BeTrue();
            lineItem.Taxes.Count.Should().Be(taxesCount - 1);
            lineItem.Taxes.Should().NotContain(toRemove);
        }

        [Fact]
        public void Return_Failure_On_RemoveTax_When_Tax_Is_Invalid()
        {
            var taxesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, taxesCount).Generate();
            lineItem.Taxes.Count.Should().Be(taxesCount);

            var result = lineItem.RemoveTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
            lineItem.Taxes.Count.Should().Be(taxesCount);
        }

        [Fact]
        public void AddPurchase()
        {
            var purchasesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, 0, purchasesCount).Generate();
            lineItem.Purchases.Count.Should().Be(purchasesCount);
            var tax = new RepairOrderPurchaseFaker().Generate();

            var result = lineItem.AddPurchase(tax);

            result.IsSuccess.Should().BeTrue();
            lineItem.Purchases.Count.Should().Be(purchasesCount + 1);
            lineItem.Purchases.Should().Contain(tax);
        }

        [Fact]
        public void Return_Failure_On_AddPurchase_When_Purchase_Is_Invalid()
        {
            var purchasesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, 0, purchasesCount).Generate();
            lineItem.Purchases.Count.Should().Be(purchasesCount);

            var result = lineItem.AddPurchase(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
        }

        [Fact]
        public void RemovePurchase()
        {
            var purchasesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, 0, purchasesCount).Generate();
            lineItem.Purchases.Count.Should().Be(purchasesCount);
            var toRemove = lineItem.Purchases[0];

            var result = lineItem.RemovePurchase(toRemove);

            result.IsSuccess.Should().BeTrue();
            lineItem.Purchases.Count.Should().Be(purchasesCount - 1);
            lineItem.Purchases.Should().NotContain(toRemove);
        }

        [Fact]
        public void Return_Failure_On_RemovePurchase_When_Purchase_Is_Invalid()
        {
            var purchasesCount = 2;
            var lineItem = new RepairOrderLineItemFaker(true, 0, 0, 0, purchasesCount).Generate();
            lineItem.Purchases.Count.Should().Be(purchasesCount);

            var result = lineItem.RemovePurchase(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderLineItem.RequiredMessage);
            lineItem.Purchases.Count.Should().Be(purchasesCount);
        }
    }
}
