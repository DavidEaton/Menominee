using CustomerVehicleManagement.Api.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace CustomerVehicleManagement.Tests.Integration
{
    /*
     * VK Im.2:
     *
     * 1) Deleting and re-creating the database on each test is slow. Use migrations to bring the tests db to the
     * desired state (manually, once), and then before each test only remove non-reference data from the database.
     * Each test then should create its fixtures/test data in its Arrange section.
     * (Reference data is data that's not changed by the app and is required for the app to run properly, e.g PhoneType etc)
     *
     * 2) Seeding data shouldn't be part of the "common" step for all integration tests. If its reference data, then
     * modify it in migration scripts. If it's master/non-reference data, then each test should create its own set of such data in Arrange section
     */
    public class IntegrationTestBase
    {
        // TODO: Move IntegrationTestsConnectionString to settings/configuration
        internal const string IntegrationTestsConnectionString = @"Server=localhost;Database=MenomineeIntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public IntegrationTestBase()
        {
            // When created, each test that inherits from this IntegrationTestBase
            // automatically clears the database, setting it to a known state.
            ClearDatabase();
        }

        internal static ApplicationDbContext CreateTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(IntegrationTestsConnectionString);

            return new ApplicationDbContext(IntegrationTestsConnectionString);
        }

        private static void ClearDatabase()
        {
            var sqlCommands = new List<string>();
            var typeNames = DatabaseTableNamesForDeletion();

            foreach (var name in typeNames)
                sqlCommands.Add($"DELETE FROM [dbo].[{name}]");

            foreach (var command in sqlCommands)
            {
                try
                {
                    ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    // Continue executing list after exception
                    // TODO: log exception
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static IList<string> DatabaseTableNamesForDeletion()
        {
            // Order matters. Use this manual list in ClearDatabase();
            // add new table names as integration tests are added
            return new List<string>
                {
                    "CreditCard",
                    "VendorInvoiceLineItem",
                    "VendorInvoicePayment",
                    "VendorInvoiceTax",
                    "ExciseFee",
                    "SalesTax",
                    "VendorInvoice",
                    "VendorInvoicePaymentMethod",
                    "Phone",
                    "Email",
                    "Vendor",

                    //"Person",
                    //"Organization",
                    //"Customer",
                };
        }

        private static void ExecuteCommand(string query)
        {
            using var connection = new SqlConnection(IntegrationTestsConnectionString);
            var command = new SqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
