using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderServiceFaker : Faker<RepairOrderService>
    {
        public RepairOrderServiceFaker(
            bool generateId = false,
            int lineItemsCount = 0,
            int techniciansCount = 0,
            int taxesCount = 0,
            long id = 0,
            SaleCode saleCodeFromCaller = null,
            Manufacturer manufacturerFromCaller = null,
            ProductCode productCodeFromCaller = null,
            RepairOrderItem repairOrderItemFromCaller = null,
            List<Employee> employees = null)

        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var serviceName = faker.Company.CompanyName();
                var saleCode = saleCodeFromCaller is null
                    ? new SaleCodeFaker(true).Generate()
                    : saleCodeFromCaller;
                var isCounterSale = faker.Random.Bool();
                var shopSuppliesTotal = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

                var lineItems = new RepairOrderLineItemFaker(generateId: generateId, id: id, manufacturerFromCaller: manufacturerFromCaller, saleCodeFromCaller: saleCodeFromCaller, productCodeFromCaller: productCodeFromCaller, repairOrderItemFromCaller: repairOrderItemFromCaller, serialNumbersCount: lineItemsCount, warrantiesCount: lineItemsCount, taxesCount: lineItemsCount, purchasesCount: lineItemsCount)
                    .Generate(lineItemsCount);

                var technicians = new RepairOrderServiceTechnicianFaker(generateId: generateId, id: id, employees: employees)
                    .Generate(techniciansCount);

                var taxes = new RepairOrderServiceTaxFaker(generateId: generateId, id: id)
                    .Generate(taxesCount);

                var service = RepairOrderService.Create(serviceName, saleCode, shopSuppliesTotal).Value;

                lineItems.ForEach(item =>
                    service.AddLineItem(item));

                technicians.ForEach(technician =>
                    service.AddTechnician(technician));

                taxes.ForEach(tax =>
                    service.AddTax(tax));

                return service;
            });
        }

    }
}
