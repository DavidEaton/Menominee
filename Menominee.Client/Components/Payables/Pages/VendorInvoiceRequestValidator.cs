using FluentValidation;
using Menominee.Shared.Models.Payables.Invoices;

namespace Menominee.Client.Components.Payables.Pages
{
    public class VendorInvoiceRequestValidator : AbstractValidator<VendorInvoiceToWrite>
    {
        public VendorInvoiceRequestValidator()
        {
        }
    }
}