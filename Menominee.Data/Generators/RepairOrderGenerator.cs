using Bogus;
using Menominee.Common.Enums;
using Menominee.Data.Database;
using Menominee.Data.Results;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.TestingHelperLibrary.Fakers;
using TestingHelperLibrary.Fakers;

namespace Menominee.Data.Generators
{
    internal class RepairOrderGenerator
    {
        public static void GenerateData()
        {
            var count = 10;

            GeneratePersons(count);

            foreach (var item in RepairOrderGeneratorResult.Persons)
                Helper.SaveToDatabase(item);

            GenerateBusinesses(count);

            foreach (var item in RepairOrderGeneratorResult.Businesses)
                Helper.SaveToDatabase(item);

            GeneratePersonCustomers();
            GenerateBusinessCustomers();

            foreach (var item in RepairOrderGeneratorResult.Customers)
                Helper.SaveToDatabase(item);

            GenerateVehicles();

            foreach (var item in RepairOrderGeneratorResult.Vehicles)
                Helper.SaveToDatabase(item);

            AddVehiclesToCustomers();

            foreach (var item in RepairOrderGeneratorResult.Customers)
                Helper.SaveToDatabase(item);

            GenerateCustomerRepairOrders();

            foreach (var item in RepairOrderGeneratorResult.RepairOrders)
                Helper.SaveToDatabase(item);
        }

        private static void GenerateVehicles()
        {
            RepairOrderGeneratorResult.Vehicles = new VehicleFaker(false).Generate(RepairOrderGeneratorResult.Customers.Count);
        }

        private static void GeneratePersons(int count)
        {
            var generateId = false;
            var persons = new PersonFaker(generateId: generateId)
                .Generate(count);

            RepairOrderGeneratorResult.Persons = persons;
        }

        private static void GenerateBusinesses(int count)
        {
            var generateId = false;
            var businesses = new BusinessFaker(generateId: generateId)
                .Generate(count);

            RepairOrderGeneratorResult.Businesses = businesses;
        }


        private static void GeneratePersonCustomers()
        {
            for (var i = 0; i < RepairOrderGeneratorResult.Persons.Count; i++)
            {
                RepairOrderGeneratorResult.Customers.Add(Customer.Create(
                    RepairOrderGeneratorResult.Persons[i],
                    CustomerType.Retail,
                    code: string.Empty)
                    .Value);
            }
        }

        private static void GenerateBusinessCustomers()
        {
            for (var i = 0; i < RepairOrderGeneratorResult.Businesses.Count; i++)
            {
                RepairOrderGeneratorResult.Customers.Add(Customer.Create(
                    RepairOrderGeneratorResult.Businesses[i],
                    CustomerType.Retail,
                    code: string.Empty)
                    .Value);
            }
        }

        private static void AddVehiclesToCustomers()
        {
            for (var i = 0; i < RepairOrderGeneratorResult.Customers.Count; i++)
                RepairOrderGeneratorResult.Customers[i].AddVehicle(RepairOrderGeneratorResult.Vehicles[i]);
        }

        private static void GenerateCustomerRepairOrders()
        {
            var faker = new Faker();
            var accountingDate = faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
            var repairOrderNumbers = new List<long>();
            var repairOrderNumber = faker.Random.Long(1000, 100000);
            var lastInvoiceNumber = faker.Random.Long(1000, 100000);

            var repairOrders = new List<RepairOrder>();
            foreach (var customer in RepairOrderGeneratorResult.Customers)
            {
                var vehicle = customer.Vehicles[0];
                var repairOrderResult = RepairOrder.Create(customer, vehicle, accountingDate, repairOrderNumbers, ++lastInvoiceNumber);

                if (repairOrderResult.IsSuccess)
                {
                    repairOrderResult.Value.SetRepairOrderNumber(++repairOrderNumber, DateTime.Now);
                    repairOrders.Add(repairOrderResult.Value);
                }
            }
            RepairOrderGeneratorResult.RepairOrders = repairOrders;
        }


        //private void SeedData()
        //{

        //    var faker = new Faker();
        //    var accountingDate = faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
        //    var repairOrderNumbers = new List<long>();
        //    var lastInvoiceNumber = faker.Random.Long(1000, 100000);



        //    var saleCodes = SaleCodeMaker.GenerateSaleCodes();
        //    dataSeeder.Save(saleCodes);

        //    var employees = new EmployeeFaker(false, count).Generate(count);
        //    dataSeeder.Save(employees);

        //    var manufacturers = new ManufacturerFaker(false).Generate(count);
        //    dataSeeder.Save(manufacturers);

        //    var saleCode = dbContext.SaleCodes.First();
        //    var manufacturer = dbContext.Manufacturers.First();
        //    var productCodes = new ProductCodeFaker(generateId, saleCodeFromCaller: saleCode, manufacturerFromCaller: manufacturer).Generate(count);
        //    dataSeeder.Save(productCodes);





        //}
    }
}