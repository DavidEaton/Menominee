using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderLineItemFaker : Faker<RepairOrderLineItem>
    {
        public RepairOrderLineItemFaker(bool generateId = false, int serialNumbersCount = 0, int warrantiesCount = 0, int taxesCount = 0, int purchasesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
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
                var serialNumbers = new RepairOrderSerialNumberFaker(generateId).Generate(serialNumbersCount);
                var warranties = new RepairOrderWarrantyFaker(generateId).Generate(warrantiesCount);
                var taxes = new RepairOrderItemTaxFaker(generateId).Generate(taxesCount);
                var purchases = new RepairOrderPurchaseFaker(generateId).Generate(purchasesCount);
                var lineItem = RepairOrderLineItem.Create(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount).Value;

                serialNumbers.ForEach(serialNumber =>
                    lineItem.AddSerialNumber(serialNumber));

                warranties.ForEach(warranty =>
                    lineItem.AddWarranty(warranty));

                taxes.ForEach(tax =>
                    lineItem.AddTax(tax));

                purchases.ForEach(purchase =>
                    lineItem.AddPurchase(purchase));

                return lineItem;
            });
        }
    }
}
