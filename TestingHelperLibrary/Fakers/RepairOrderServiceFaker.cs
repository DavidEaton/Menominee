using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderServiceFaker : Faker<RepairOrderService>
    {
        public RepairOrderServiceFaker(bool generateId = false, int lineItemsCount = 0, int techniciansCount = 0, int taxesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var serviceName = faker.Company.CompanyName();
                var saleCode = new SaleCodeFaker(true).Generate();
                var isCounterSale = faker.Random.Bool();
                var shopSuppliesTotal = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

                var lineItems = new RepairOrderLineItemFaker(generateId).Generate(lineItemsCount);
                var technicians = new RepairOrderServiceTechnicianFaker(generateId).Generate(techniciansCount);
                var taxes = new RepairOrderServiceTaxFaker(generateId).Generate(taxesCount);

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
