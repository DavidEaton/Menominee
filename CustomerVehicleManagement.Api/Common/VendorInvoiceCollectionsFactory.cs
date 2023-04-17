using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using System.Collections.Generic;
using System.Linq;
using CustomerVehicleManagement.Shared.Models.Taxes;

namespace CustomerVehicleManagement.Api.Common
{
    public static class VendorInvoiceCollectionsFactory
    {
        // TODO: VK Question: Is this how you would go about updating colections?
        public static VendorInvoiceCollections Create(
            IReadOnlyList<VendorInvoiceLineItemToWrite> lineItemsToWrite,
            IReadOnlyList<VendorInvoicePaymentToWrite> paymentsToWrite,
            IReadOnlyList<VendorInvoiceTaxToWrite> taxesToWrite,
            IReadOnlyList<Manufacturer> manufacturers,
            IReadOnlyList<SaleCode> saleCodes)
        {
            VendorInvoiceLineItem[] lineItems = (lineItemsToWrite ?? new List<VendorInvoiceLineItemToWrite>())
                .Select(lineItem =>
                    VendorInvoiceLineItem.Create(
                        lineItem.Type,
                        VendorInvoiceItemHelper.ConvertWriteDtoToEntity(lineItem.Item, manufacturers, saleCodes),
                        lineItem.Quantity,
                        lineItem.Cost,
                        lineItem.Core,
                        lineItem.PONumber,
                        lineItem.TransactionDate)
                    .Value)
                .ToArray();

            VendorInvoicePayment[] payments = (paymentsToWrite ?? new List<VendorInvoicePaymentToWrite>())
                .Select(payment =>
                    VendorInvoicePayment.Create(
                        VendorInvoicePaymentHelper.ConvertWriteDtoToEntity(payment.PaymentMethod),
                        payment.Amount)
                    .Value)
                .ToArray();

            VendorInvoiceTax[] taxes = (taxesToWrite ?? new List<VendorInvoiceTaxToWrite>())
                .Select(tax =>
                    VendorInvoiceTax.Create(
                        SalesTaxHelper.ConvertReadDtoToEntity(tax.SalesTax),
                        tax.Amount)
                    .Value)
                .ToArray();

            return new VendorInvoiceCollections(lineItems, payments, taxes);
        }
    }
}
