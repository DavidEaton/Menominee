﻿using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CustomerVehicleManagement.Data.Database
{
    internal static class Helper
    {
        internal const string ConnectionString = @"Server=localhost;Database=Menominee-stage;Trusted_Connection=True;";
        internal static int savedVendors = 0;
        internal static int savedVendorInvoices = 0;
        internal static int savedInventoryItems = 0;
        internal static bool EnsureDeletedEnsureCreated { get; private set; } = true;

        internal static void SaveToDatabase(Vendor vendor)
        {
            using (var context = new ApplicationDbContext(ConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                    context.Database.OpenConnection();

                try
                {
                    context.Vendors.Attach(vendor);
                    context.SaveChanges();
                    savedVendors++;
                }
                catch (Exception ex)
                {
                    // Continue after failed insert
                    Console.WriteLine($"failed insert: {ex.Message}");
                    Console.WriteLine();
                    Console.WriteLine("Continuing with retry...");
                    Console.WriteLine();
                }
            }

            Console.WriteLine($"Saved {savedVendors} Vendor rows.");
        }

        internal static void SaveToDatabase(VendorInvoice vendorInvoice)
        {
            using (var context = new ApplicationDbContext(ConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                    context.Database.OpenConnection();

                try
                {
                    context.VendorInvoices.Attach(vendorInvoice);
                    context.SaveChanges();
                    savedVendorInvoices++;
                }
                catch (Exception ex)
                {
                    // Continue after failed insert
                    Console.WriteLine($"failed insert: {ex.Message}");
                    Console.WriteLine();
                    Console.WriteLine("Continuing with retry...");
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"Saved {savedVendorInvoices} VendorInvoice rows.");
        }

        internal static void SaveToDatabase(InventoryItem inventoryItem)
        {
            using (var context = new ApplicationDbContext(ConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                    context.Database.OpenConnection();

                try
                {
                    context.InventoryItems.Attach(inventoryItem);
                    context.SaveChanges();
                    savedInventoryItems++;
                }
                catch (Exception ex)
                {
                    // Continue after failed insert
                    Console.WriteLine($"failed insert: {ex.Message}");
                    Console.WriteLine();
                    Console.WriteLine("Continuing with retry...");
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"Saved {savedInventoryItems} inventoryItem rows.");
        }

        internal static IReadOnlyList<Vendor> GetVendors()
        {
            IReadOnlyList<Vendor> vendorsFromContext = new List<Vendor>();

            using (var context = new ApplicationDbContext(ConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                    context.Database.OpenConnection();

                try
                {
                    vendorsFromContext = context.Vendors
                        .Include(vendor => vendor.DefaultPaymentMethod.PaymentMethod)
                        .AsNoTracking()
                        .ToList();
                }
                catch (Exception ex)
                {
                    // Continue after failed insert
                    Console.WriteLine($"failed insert: {ex.Message}");
                    Console.WriteLine();
                    Console.WriteLine("Continuing with retry...");
                    Console.WriteLine();
                }
            }

            return vendorsFromContext;
        }

        internal static void ClearDatabase()
        {
            using (var context = new ApplicationDbContext(ConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    try
                    {
                        // Will fail if database does not exist
                        context.Database.OpenConnection();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"failed ClearDatabase(): {ex.Message}");

                        Console.WriteLine();
                        Console.WriteLine($"Creating database...");
                        Console.WriteLine();
                        context.Database.EnsureCreated();
                        EnsureDeletedEnsureCreated = false;
                    }
                }

                try
                {
                    if (EnsureDeletedEnsureCreated)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Deleting database...");
                        Console.WriteLine();
                        context.Database.EnsureDeleted();

                        Console.WriteLine();
                        Console.WriteLine($"Creating database...");
                        Console.WriteLine();
                        context.Database.EnsureCreated();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"failed ClearDatabase(): {ex.Message}");
                    Console.WriteLine();
                }
            }
        }
    }
}
