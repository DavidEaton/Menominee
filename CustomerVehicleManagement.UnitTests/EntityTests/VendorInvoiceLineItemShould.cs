using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceLineItemShould
    {
        [Fact]
        public void Create_VendorInvoiceLineItem()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;
            var item = VendorInvoiceItem.Create("BR549", "a description", manufacturer, saleCode).Value;

            // Act
            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1);

            // Assert
            lineItemOrError.Value.Should().BeOfType<VendorInvoiceLineItem>();
            lineItemOrError.IsFailure.Should().BeFalse();
            lineItemOrError.Value.Item.Manufacturer.Should().Be(manufacturer);
            lineItemOrError.Value.Item.SaleCode.Should().Be(saleCode);
            lineItemOrError.Value.Item.SaleCode.ShopSupplies.Should().Be(saleCode.ShopSupplies);
        }

        [Fact]
        public void Create_VendorInvoiceLineItem_With_Optional_PONumber()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var poNumber = "001";

            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, poNumber).Value;

            lineItem.Should().NotBeNull();
            lineItem.Should().BeOfType<VendorInvoiceLineItem>();
            lineItem.PONumber.Should().Be(poNumber);
        }

        [Fact]
        public void Create_VendorInvoiceLineItem_With_Optional_TransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var transactionDate = DateTime.Today;

            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, transactionDate: transactionDate).Value;

            lineItem.Should().NotBeNull();
            lineItem.Should().BeOfType<VendorInvoiceLineItem>();
            lineItem.TransactionDate.Should().Be(transactionDate);
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_VendorInvoiceItemType()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidType = (VendorInvoiceItemType)(-1);

            var lineItemOrError = VendorInvoiceLineItem.Create(invalidType, item, 1, 1, 1);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Null_VendorInvoiceItem()
        {
            VendorInvoiceItem nullItem = null;

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, nullItem, 1, 1, 1);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_Quantity()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidQuantity = -1.0;

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, invalidQuantity, 1, 1);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_Cost()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidCost = VendorInvoiceLineItem.MinimumValue - 1;

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, invalidCost, 1);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_Core()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidCore = VendorInvoiceLineItem.MinimumValue - 1;

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, invalidCore);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_PONumber()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidPONumber = Utilities.RandomCharacters(VendorInvoiceLineItem.PONumberMaximumLength + 1);

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, invalidPONumber);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoiceLineItem_With_Invalid_TransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidTransactionDate = DateTime.Today.AddDays(1);

            var lineItemOrError = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, transactionDate: invalidTransactionDate);

            lineItemOrError.IsFailure.Should().BeTrue();
            lineItemOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void SetType()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var type = VendorInvoiceItemType.Purchase;
            var lineItem = VendorInvoiceLineItem.Create(type, item, 1, 1, 1).Value;
            var typeSet = VendorInvoiceItemType.CoreReturn;
            lineItem.Type.Should().Be(type);

            lineItem.SetType(typeSet);

            lineItem.Type.Should().Be(typeSet);
        }

        [Fact]
        public void SetItem()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var itemSet = VendorInvoiceItem.Create("BR549Replacement", "another description").Value;
            lineItem.Item.Should().Be(item);

            lineItem.SetItem(itemSet);

            lineItem.Item.Should().Be(itemSet);
        }

        [Fact]
        public void SetQuantity()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var quantity = 1.0;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, quantity, 1, 1).Value;
            var quantitySet = 2.0;
            lineItem.Quantity.Should().Be(quantity);

            lineItem.SetQuantity(quantitySet);

            lineItem.Quantity.Should().Be(quantitySet);
        }

        [Fact]
        public void SetCost()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var cost = 1.0;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, cost, 1, 1).Value;
            var costSet = 2.0;
            lineItem.Cost.Should().Be(cost);

            lineItem.SetCost(costSet);

            lineItem.Cost.Should().Be(costSet);
        }

        [Fact]
        public void SetCore()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var core = 1.0;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, core).Value;
            var coreSet = 2.0;
            lineItem.Core.Should().Be(core);

            lineItem.SetCore(coreSet);

            lineItem.Core.Should().Be(coreSet);
        }

        [Fact]
        public void SetPONumber()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var poNumber = "001";
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, poNumber: poNumber).Value;
            var poNumberSet = "002";
            lineItem.PONumber.Should().Be(poNumber);

            lineItem.SetPONumber(poNumberSet);

            lineItem.PONumber.Should().Be(poNumberSet);
        }

        [Fact]
        public void ClearPONumber()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var poNumber = "001";
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, poNumber: poNumber).Value;
            lineItem.PONumber.Should().Be(poNumber);

            lineItem.ClearPONumber();

            lineItem.PONumber.Should().Be(string.Empty);
        }

        [Fact]
        public void SetTransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var transactionDate = DateTime.Today;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            lineItem.TransactionDate.Should().BeNull();

            lineItem.SetTransactionDate(transactionDate);

            lineItem.TransactionDate.Should().Be(transactionDate);
        }

        [Fact]
        public void ClearTransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var transactionDate = DateTime.Today;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, transactionDate: transactionDate).Value;
            lineItem.TransactionDate.Should().Be(transactionDate);
            
            lineItem.ClearTransactionDate();

            lineItem.TransactionDate.Should().BeNull();
        }

        [Fact]
        public void Not_Set_Invalid_Type()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var invalidType = (VendorInvoiceItemType)(-1);
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;

            var resultOrError = lineItem.SetType(invalidType);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_Item()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            VendorInvoiceItem nullItem = null;

            var resultOrError = lineItem.SetItem(nullItem);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Quantity()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var invalidQuantity = VendorInvoiceLineItem.MinimumValue - 1;

            var resultOrError = lineItem.SetQuantity(invalidQuantity);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Cost()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var invalidCost = -.01;

            var resultOrError = lineItem.SetCost(invalidCost);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Core()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var invalidCore = -.01;
            var resultOrError = lineItem.SetCore(invalidCore);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_PONumber(int length)
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var invalidPONumber = Utilities.RandomCharacters(length);

            var resultOrError = lineItem.SetPONumber(invalidPONumber);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_TransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            DateTime? nullTransactionDate = null;

            var resultOrError = lineItem.SetTransactionDate(nullTransactionDate);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_TransactionDate()
        {
            var item = VendorInvoiceItem.Create("BR549", "a description").Value;
            var lineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1).Value;
            var invalidTransactionDate = DateTime.Today.AddDays(1);

            var resultOrError = lineItem.SetTransactionDate(invalidTransactionDate);

            resultOrError.IsFailure.Should().BeTrue();
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { VendorInvoiceLineItem.MinimumValue };
                    yield return new object[] { VendorInvoiceLineItem.PONumberMaximumLength + 1 };
                }
            }
        }
    }
}
