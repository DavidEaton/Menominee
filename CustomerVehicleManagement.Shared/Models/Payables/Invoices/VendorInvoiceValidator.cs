using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public class VendorInvoiceValidator : AbstractValidator<VendorInvoiceToWrite>
    {
        public VendorInvoiceValidator()
        {
            IReadOnlyList<string> vendorInvoiceNumbers = new List<string>();

            RuleFor(invoice => invoice)
                .MustBeEntity(
                    invoice => VendorInvoice.Create(
                        Vendor.Create(
                            invoice.Vendor.Name,
                            invoice.Vendor.VendorCode,
                            invoice.Vendor.VendorRole,
                            invoice.Vendor.Notes
                            ).Value,
                        invoice.Status,
                        invoice.DocumentType,
                        invoice.Total,
                        vendorInvoiceNumbers,
                        invoice.InvoiceNumber,
                        invoice.Date,
                        invoice.DatePosted
                        ));

            RuleFor(invoice => invoice.LineItems)
                .SetValidator(new VendorInvoiceLineItemsValidator())
                .When(invoice => invoice.LineItems is not null
                                && invoice.LineItems.Count > 0);

            RuleFor(invoice => invoice.Payments)
                .SetValidator(new VendorInvoicePaymentsValidator())
                .When(invoice => invoice.Payments is not null
                                && invoice.Payments.Count > 0);

            RuleFor(invoice => invoice.Taxes)
                .SetValidator(new VendorInvoiceTaxesValidator())
                .When(invoice => invoice.Taxes is not null
                                && invoice.Taxes.Count > 0);
        }
    }
}
