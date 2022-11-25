using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
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
            if (invoice is not null)
                context.Attach(invoice);
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices.FindAsync(id);
            if (invoiceFromContext != null)
                context.Remove(invoiceFromContext);
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

        internal void InspectTrackingStates(VendorInvoice invoice)
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

        public async Task<IReadOnlyList<string>> GetVendorInvoiceNumbers(long vendorId)
        {
            return await context.VendorInvoices
                .Where(invoice => invoice.Vendor.Id == vendorId)
                .Select(invoice => $"{invoice.Vendor.Id}{invoice.InvoiceNumber}")
                    .Where(invoiceNumber => !string.IsNullOrWhiteSpace(invoiceNumber))
                .ToListAsync();
        }
    }
}
