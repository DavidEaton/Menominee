using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public class VendorInvoiceRepository : IVendorInvoiceRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void AddInvoice(VendorInvoice invoice)
        {
            // Inspect tracking states BEFORE attaching invoice to context;
            // attaching invoice to context makes EF start tracking changes.
            //InspectTrackingStates(invoice);

            // Add() marks the entire aggregate and its childernas Added;
            // use Attach() instead.
            if (invoice != null)
                context.Attach(invoice);

            // Inspect tracking states AFTER attaching invoice to context.
            //InspectTrackingStates(invoice);

            // EF Core 6 unfortunately (and incorrectly) marks EXISTING
            // entities as Added when using Add(); Prevent updating Entity
            // type navigation properties by setting those reference items
            // to Unchanged.
            foreach (var vendor in context.ChangeTracker.Entries<Vendor>())
                vendor.State = EntityState.Unchanged;


            //// FAILS TO MARK THE EXISTING VendorInvoicePaymentMethod ?
            //foreach (var paymentMethod in context.ChangeTracker.Entries<VendorInvoicePaymentMethod>())
            //    paymentMethod.State = EntityState.Unchanged;

            //foreach (var salesTax in context.ChangeTracker.Entries<SalesTax>())
            //    salesTax.State = EntityState.Unchanged;

            //foreach (var manufacturer in context.ChangeTracker.Entries<Manufacturer>())
            //    manufacturer.State = EntityState.Unchanged;

            //foreach (var saleCode in context.ChangeTracker.Entries<SaleCode>())
            //    saleCode.State = EntityState.Unchanged;

            //foreach (var supplies in context.ChangeTracker.Entries<SaleCodeShopSupplies>())
            //    supplies.State = EntityState.Unchanged;

            //foreach (var exciseFee in context.ChangeTracker.Entries<ExciseFee>())
            //    exciseFee.State = EntityState.Unchanged;

            // Inspect tracking states after CORRECTING tracking states.
            InspectTrackingStates(invoice);
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices.FindAsync(id);
            if (invoiceFromContext != null)
                context.Remove(invoiceFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<VendorInvoiceToRead> GetInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices
                                                  .Include(invoice => invoice.Vendor)
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.Item.Manufacturer)
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.Item.SaleCode)
                                                  .Include(invoice => invoice.Payments)
                                                      .ThenInclude(payment => payment.PaymentMethod)
                                                          .ThenInclude(method => method.ReconcilingVendor)
                                                  .Include(invoice => invoice.Taxes)
                                                      .ThenInclude(tax => tax.SalesTax)
                                                  .AsSplitQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return invoiceFromContext is null
            ? null
            : VendorInvoiceHelper.ConvertEntityToReadDto(invoiceFromContext);
        }

        public async Task<Vendor> GetVendorAsync(long id)
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        public async Task<VendorInvoice> GetInvoiceEntityAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.Manufacturer)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.SaleCode)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                        .ThenInclude(method => method.ReconcilingVendor)
                .Include(invoice => invoice.Taxes)
                    .ThenInclude(tax => tax.SalesTax)
                .AsSplitQuery()
                .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return invoiceFromContext;
        }

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync()
        {
            IReadOnlyList<VendorInvoice> invoices = await context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return invoices.Select(invoice => VendorInvoiceHelper.ConvertEntityToReadInListDto(invoice))
                                   .ToList();
        }

        public async Task<bool> InvoiceExistsAsync(long id)
        {
            return await context.VendorInvoices.AnyAsync(invoice => invoice.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void DeleteInvoice(VendorInvoice invoice)
        {
            context.Remove(invoice);
        }

        public void InspectTrackingStates(VendorInvoice invoice)
        {
            EntityState stateInvoice = context.Entry(invoice).State;
            TrackingState trackingStateInvoice = invoice.TrackingState;
            EntityState stateVendor = context.Entry(invoice.Vendor).State;
            TrackingState trackingStateVendor = invoice.Vendor.TrackingState;

            foreach (var lineItem in invoice.LineItems)
            {
                EntityState stateLineItem = context.Entry(lineItem).State;
                TrackingState trackingStateLineItem = lineItem.TrackingState;

                EntityState stateItem = context.Entry(lineItem.Item).State;
                TrackingState trackingStateItem = lineItem.Item.TrackingState;

                if (lineItem.Item.Manufacturer is not null)
                {
                    EntityState stateItemManufacturer = context.Entry(lineItem.Item.Manufacturer).State;
                    TrackingState trackingStateItemManufacturer = lineItem.Item.Manufacturer.TrackingState;
                }

                if (lineItem.Item.SaleCode is not null)
                {
                    EntityState stateItemSaleCode = context.Entry(lineItem.Item.SaleCode).State;
                    TrackingState trackingStateItemSaleCode = lineItem.Item.SaleCode.TrackingState;
                }
            }

            foreach (var payment in invoice.Payments)
            {
                EntityState statePayment = context.Entry(payment).State;
                TrackingState trackingStatePayment = payment.TrackingState;

                EntityState statePaymentMethod = context.Entry(payment.PaymentMethod).State;
                TrackingState trackingStatePaymentMethod = payment.PaymentMethod.TrackingState;

                if (payment.PaymentMethod.ReconcilingVendor is not null)
                {
                    EntityState statePaymentReconcilingVendor = context.Entry(payment.PaymentMethod.ReconcilingVendor).State;
                    TrackingState trackingStatePaymentReconcilingVendor = payment.PaymentMethod.ReconcilingVendor.TrackingState;
                }
            }

            foreach (var tax in invoice.Taxes)
            {
                EntityState stateTax = context.Entry(tax).State;
                TrackingState trackingStateTax = tax.TrackingState;

                EntityState stateSalesTax = context.Entry(tax.SalesTax).State;
                TrackingState trackingStateSalesTax = tax.SalesTax.TrackingState;

                foreach (var fee in tax.SalesTax.ExciseFees)
                {
                    EntityState stateSalesTaxFee = context.Entry(fee).State;
                    TrackingState trackingStateSalesTaxFee = fee.TrackingState;
                }
            }
        }

    }
}
