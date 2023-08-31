using Bogus;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using System;
using System.Linq;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderServiceShould
    {
        [Fact]
        public void Create_RepairOrderService()
        {
            var faker = new Faker();
            var serviceName = faker.Company.CompanyName();
            var saleCode = new SaleCodeFaker(true).Generate();
            var shopSuppliesTotal = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

            var result = RepairOrderService.Create(serviceName, saleCode, shopSuppliesTotal);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void SetServiceName()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var newName = GenerateNewServiceName(service.ServiceName);

            var result = service.SetServiceName(newName);

            result.IsSuccess.Should().BeTrue();
            service.ServiceName.Should().Be(newName);
        }

        private static string GenerateNewServiceName(string currentName)
        {
            var newName = new Faker().Company.CompanyName();

            return newName.Equals(currentName)
                ? GenerateNewServiceName(currentName)
                : newName;
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_ServiceName()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var invalidName = new Faker().Random.Utf16String(RepairOrderService.MaximumLength + 1, RepairOrderService.MaximumLength + 1);

            var result = service.SetServiceName(invalidName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.InvalidLengthMessage);
        }

        [Fact]
        public void SetSaleCode()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var newSaleCode = new SaleCodeFaker(true).Generate();

            var result = service.SetSaleCode(newSaleCode);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newSaleCode);
            service.SaleCode.Should().Be(newSaleCode);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_SaleCode()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.SetSaleCode(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void AddLineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();

            var result = service.AddLineItem(lineItem);

            result.IsSuccess.Should().BeTrue();
            service.LineItems.Should().Contain(lineItem);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_LineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddLineItem(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void RemoveLineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();
            service.AddLineItem(lineItem);

            var result = service.RemoveLineItem(lineItem);

            result.IsSuccess.Should().BeTrue();
            service.LineItems.Should().NotContain(lineItem);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_LineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();
            service.AddLineItem(lineItem);

            var result = service.RemoveLineItem(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void AddTechnician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();

            var result = service.AddTechnician(technician);

            result.IsSuccess.Should().BeTrue();
            service.Technicians.Should().Contain(technician);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Technician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddTechnician(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void RemoveTechnician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();
            service.AddTechnician(technician);

            var result = service.RemoveTechnician(technician);

            result.IsSuccess.Should().BeTrue();
            service.Technicians.Should().NotContain(technician);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Technician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();
            service.AddTechnician(technician);

            var result = service.RemoveTechnician(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void AddTax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();

            var result = service.AddTax(tax);

            result.IsSuccess.Should().BeTrue();
            service.Taxes.Should().Contain(tax);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Tax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Fact]
        public void RemoveTax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();
            service.AddTax(tax);

            var result = service.RemoveTax(tax);

            result.IsSuccess.Should().BeTrue();
            service.Taxes.Should().NotContain(tax);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Tax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();
            service.AddTax(tax);

            var result = service.RemoveTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderService.RequiredMessage);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Return_Correct_IsCounterSale(bool isCounterSale)
        {
            var service = new RepairOrderServiceFaker(true, lineItemsCount: 3).Generate();

            foreach (var lineItem in service.LineItems)
                lineItem.SetIsCounterSale(isCounterSale);

            service.IsCounterSale.Should().Be(isCounterSale);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Return_Correct_IsDeclined(bool isDeclined)
        {
            var service = new RepairOrderServiceFaker(true, lineItemsCount: 3).Generate();

            foreach (var lineItem in service.LineItems)
                lineItem.SetIsDeclined(isDeclined);

            service.IsDeclined.Should().Be(isDeclined);
        }

        [Fact]
        public void Return_Correct_PartsTotal()
        {
            var rowCount = 3;
            var quantity = 3;
            var amount = 11.11;
            var generateId = true;
            var service = new RepairOrderServiceFaker(generateId, lineItemsCount: rowCount).Generate();

            foreach (var lineItem in service.LineItems)
            {
                lineItem.SetQuantitySold(quantity);
                lineItem.SetLaborAmount(LaborAmount.Create(ItemLaborType.Flat, amount).Value);
                lineItem.SetSellingPrice(amount);
                lineItem.SetDiscountAmount(DiscountAmount.Create(ItemDiscountType.None, 0).Value);
            }

            var expectedPartsTotal = rowCount * quantity * (amount + amount);

            service.PartsTotal.Should().Be(expectedPartsTotal);
        }

        [Fact]
        public void Return_Correct_LaborTotal()
        {
            var rowCount = 3;
            var quantity = 3;
            var amount = 11.11;
            var generateId = true;
            var service = new RepairOrderServiceFaker(generateId, lineItemsCount: rowCount).Generate();

            foreach (var lineItem in service.LineItems)
            {
                lineItem.SetQuantitySold(quantity);
                lineItem.SetLaborAmount(LaborAmount.Create(ItemLaborType.Flat, amount).Value);
            }

            var expectedLaborTotal = rowCount * quantity * amount;

            service.LaborTotal.Should().Be(expectedLaborTotal);
        }

        [Fact]
        public void Return_Correct_DiscountTotal()
        {
            var rowCount = 3;
            var quantity = 3;
            var amount = 11.11;
            var generateId = true;
            var service = new RepairOrderServiceFaker(generateId, lineItemsCount: rowCount).Generate();

            foreach (var lineItem in service.LineItems)
            {
                lineItem.SetQuantitySold(quantity);
                lineItem.SetDiscountAmount(DiscountAmount.Create(ItemDiscountType.Dollar, amount).Value);
            }

            var expectedDiscountTotal = rowCount * quantity * amount;

            service.DiscountTotal.Should().Be(expectedDiscountTotal);
        }

        [Fact]
        public void Return_Correct_ExciseFeesTotal()
        {
            var rowCount = 3;
            var quantity = 3;
            var amount = 11.11;
            var generateId = true;
            var service = new RepairOrderServiceFaker(generateId, lineItemsCount: rowCount).Generate();
            var part = new RepairOrderItemPartFaker(generateId).Generate();
            var fees = new ExciseFeeFaker(generateId).Generate(rowCount);

            foreach (var fee in fees)
            {
                fee.SetAmount(amount);
                part.AddExciseFee(fee);
            }

            foreach (var lineItem in service.LineItems)
            {
                lineItem.SetQuantitySold(quantity);
                lineItem.Item.SetPart(part);
            }

            var expectedExciseFeesTotal = rowCount * quantity * amount;

            service.ExciseFeesTotal.Should().Be(expectedExciseFeesTotal);
        }

        [Fact]
        public void Return_Correct_LineItemsTaxTotal()
        {
            var rowCount = 3;
            var quantity = 3;
            var rate = .9;
            var amount = 11.11;
            var generateId = true;
            var service = new RepairOrderServiceFaker(generateId, lineItemsCount: rowCount).Generate();

            foreach (var lineItem in service.LineItems)
            {
                foreach (var tax in lineItem.Taxes)
                {
                    tax.SetLaborTax(LaborTax.Create(rate, amount).Value);
                    tax.SetPartTax(PartTax.Create(rate, amount).Value);
                }
            }

            var expectedLineItemsTaxTotal = rowCount * quantity * ((amount * rate) + (amount * rate));

            service.LineItemTaxesTotal.Should().Be(expectedLineItemsTaxTotal);
        }

        [Fact]
        public void Return_Correct_Totals()
        {
            var service = new RepairOrderServiceFaker(true, lineItemsCount: 3, taxesCount: 2).Generate();


            var lineItemsTaxTotal = service.LineItems.Select(
                lineItem => lineItem.Taxes
                .Sum(tax => tax.LaborTax.Amount + tax.PartTax.Amount))
                .Sum();
            var taxTotal = service.Taxes.Select(
                tax => tax.LaborTax.Amount + tax.PartTax.Amount)
                .Sum();
            var shopSuppliesTotal = service.ShopSuppliesTotal;
            //service.ExciseFeesTotal.Should().Be(exciseFeesTotal);
            //service.TaxTotal.Should().Be(taxTotal);
            //service.ShopSuppliesTotal.Should().Be(shopSuppliesTotal);
            //service.Total.Should().Be(total);
            //service.TaxTotal.Should().Be(taxTotal);
            //service.TotalWithTax.Should().Be(totalWithTax);
            //service.LineItemsTaxTotal.Should().Be(lineItemsTaxTotal);

        }

    }
}
