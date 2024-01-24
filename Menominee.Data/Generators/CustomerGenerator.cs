using Menominee.Data.Database;
using Menominee.Data.Results;
using TestingHelperLibrary.Fakers;

namespace Menominee.Data.Generators
{
    public static class CustomerGenerator
    {
        public static void GenerateData(int count)
        {
            GenerateCustomers(count);

            foreach (var customer in CustomerGeneratorResult.Customers)
            {
                Helper.SaveToDatabase(customer);
            }
        }

        private static void GenerateCustomers(int count)
        {
            var random = new Random();
            var includeAddress = true;
            var includeContact = false;
            var emailsCount = random.Next(1, 4);
            var phonesCount = random.Next(1, 4);
            var vehiclesCount = random.Next(1, 4);

            var customers = new CustomerFaker(
                generateId: true,
                includeAddress: includeAddress,
                includeContact: includeContact,
                emailsCount: emailsCount,
                phonesCount: phonesCount,
                vehiclesCount: vehiclesCount)
                .Generate(count);

            CustomerGeneratorResult.Customers = customers;
        }
    }
}
