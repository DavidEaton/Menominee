using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxesValidator : AbstractValidator<IList<VendorInvoiceTaxToWrite>>
    {
        // May be better to inject SalesTax respoitory to get
        // that entity, which when successful, validates the SalesTax
        // dto, for aggregate root validation completeness.
        // HOWEVER, that would create a circular dependency from this project to API
        // and back again. So we miss an edge case if the client somehow sends a
        // SalesTax read dto that represents a non-existent entity

        // It's such a pain to have to convert read dtos to Entities.
        // Lots of time and effort converting ToRead objects. Most of the
        // code below was written to convert the 
        // SalesTax read dto into an entity. And where do we stop? SalesTax
        // has an exciseFees collection. Create that too? No, because we are
        // here to validate VendorInvoiceTax. SalesTax represents an existing
        // entity in the database, and doesn't need validation.
        // Since the SalesTax dto isn't used for creation or update of SalesTax
        // (we're validating its parent, the domain aggregate root
        // VendorInvoiceTax), it need only be Created with as little code as
        // possible.

        private const string notEmptyMessage = "Tax must not be empty.";

        public VendorInvoiceTaxesValidator()
        {
            RuleFor(taxes => taxes)
                .NotNull()
                //.Must(HaveOnlyOnePrimaryEmail) // passs the collection to a function that returns bool: true==success
                //.WithMessage(onePrimarymessage) // override default failure message if bool returns: false==failure
                .ForEach(tax =>
                {
                    tax.NotEmpty().WithMessage(notEmptyMessage);
                    tax.MustBeEntity(
                        tax =>
                        VendorInvoiceTax.Create(
                            SalesTax.Create(
                                tax.SalesTax.Description,
                                tax.SalesTax.TaxType,
                                tax.SalesTax.Order,
                                tax.SalesTax.TaxIdNumber,
                                tax.SalesTax.PartTaxRate,
                                tax.SalesTax.LaborTaxRate)
                            .Value,
                            tax.Amount
                            ));
                });
        }
    }
}
