using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerVehicleManagement.Tests.Integration
{
    internal static class Helpers
    {
        internal const string IntegrationTestsConnectionString = @"Server=localhost;Database=MenomineeIntegrationTests;Trusted_Connection=True;";

        internal static ApplicationDbContext CreateTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(IntegrationTestsConnectionString);

            return new ApplicationDbContext(IntegrationTestsConnectionString);
        }

        internal static Vendor CreateVendor(ApplicationDbContext context)
        {

            var vendorOrError = Vendor.Create("Test Vendor", "TV-1");

            if (vendorOrError.IsFailure)
                throw new NotImplementedException();

            context.Vendors.Add(vendorOrError.Value);
            context.SaveChanges();

            return context.Vendors.Find(vendorOrError.Value.Id);
        }
    }
}
