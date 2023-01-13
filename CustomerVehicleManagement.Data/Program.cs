using CustomerVehicleManagement.Data.Generators;

namespace CustomerVehicleManagement.Data
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            VendorGenerator.GenerateData();
            VendorInvoiceGenerator.GenerateData();

            Console.ReadLine();
        }
    }
}