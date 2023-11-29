using FluentValidation;
using Menominee.Api.Features.Payables.PaymentMethods;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices;
using System.Collections.Generic;

namespace Menominee.Api.Features.Payables.Invoices
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
