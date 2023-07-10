using Menominee.Data.Generators;

namespace Menominee.Data
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            var message = "Database will be deleted, recreated and seeded with data. Please press 1 to proceed, or any other key to cancel";
            Console.WriteLine(message);

            var input = Console.ReadLine();
            input = (input ?? string.Empty).Trim();

            if (input == "1")
            {
                VendorGenerator.GenerateData();
                VendorInvoiceGenerator.GenerateData();
                InventoryItemGenerator.GenerateData();

                Console.ReadLine();
            }

        }
    }
}