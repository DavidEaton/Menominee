using Menominee.Data.Database;
using Menominee.Data.Generators;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace Menominee.Data
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(new JsonFormatter(), @"menominee-data-log-.json", rollingInterval: RollingInterval.Day)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

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
                RepairOrderGenerator.GenerateData();
                CustomerGenerator.GenerateData(25);

                Console.WriteLine(complete);
                Console.ReadLine();
            }

        }
    }
}