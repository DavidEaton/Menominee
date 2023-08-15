using Menominee.Data.Database;
using Menominee.Data.Generators;

namespace Menominee.Data
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            var warning = "Database will be deleted, recreated and seeded with data. Please press 1 to proceed, or any other key to cancel";
            var complete = "Database has been deleted, recreated and seeded with data. Press any key to exit.";
            Console.WriteLine(warning);

            var input = Console.ReadLine();
            input = (input ?? string.Empty).Trim();

            if (input == "1")
            {
                Helper.DeleteAndMigrateDatabase();
                VendorGenerator.GenerateData();
                VendorInvoiceGenerator.GenerateData();
                InventoryItemGenerator.GenerateData();

                Console.WriteLine(complete);
                Console.ReadLine();
            }

        }
    }
}