using CustomerVehicleManagement.Data.Generators;

namespace CustomerVehicleManagement.Data
{
    public class Program
    {
        internal static async Task Main(string[] args)
        {
            //VendorGenerator.GenerateData();
            await VendorInvoiceGenerator.GenerateDataAsync();

            Console.ReadLine();
        }
    }
}