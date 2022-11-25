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

        internal static void InspectTrackingStates(VendorInvoice invoice, ApplicationDbContext context)
        {
            EntityState stateInvoice = context.Entry(invoice).State;
            EntityState stateVendor = context.Entry(invoice.Vendor).State;

            foreach (var lineItem in invoice.LineItems)
            {
                EntityState stateLineItem = context.Entry(lineItem).State;
                EntityState stateItem = context.Entry(lineItem.Item).State;

                if (lineItem.Item.Manufacturer is not null)
                {
                    EntityState stateItemManufacturer = context.Entry(lineItem.Item.Manufacturer).State;
                }

                if (lineItem.Item.SaleCode is not null)
                {
                    EntityState stateItemSaleCode = context.Entry(lineItem.Item.SaleCode).State;
                }
            }

            foreach (var payment in invoice.Payments)
            {
                EntityState statePayment = context.Entry(payment).State;
                EntityState statePaymentMethod = context.Entry(payment.PaymentMethod).State;

                if (payment.PaymentMethod.ReconcilingVendor is not null)
                {
                    EntityState statePaymentReconcilingVendor = context.Entry(payment.PaymentMethod.ReconcilingVendor).State;
                }
            }

            foreach (var tax in invoice.Taxes)
            {
                EntityState stateTax = context.Entry(tax).State;
                EntityState stateSalesTax = context.Entry(tax.SalesTax).State;

                foreach (var fee in tax.SalesTax.ExciseFees)
                {
                    EntityState stateSalesTaxFee = context.Entry(fee).State;
                }
            }
        }
    }
}
