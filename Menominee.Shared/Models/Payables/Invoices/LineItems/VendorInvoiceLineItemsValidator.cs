using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Payables;
using FluentValidation;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemsValidator : AbstractValidator<IList<VendorInvoiceLineItemToWrite>>
    {
        private const string notEmptyMessage = "Line Item must not be empty.";

        public VendorInvoiceLineItemsValidator()
        {
            // TODO: Replace empty shopSupplies with injected repository call to real SaleCodeShopSupplies from database
            var shopSupplies = SaleCodeShopSupplies.Create(0, 0, 0, 0, true, true).Value;

            RuleFor(lineItems => lineItems)
                .NotNull()
                .ForEach(lineItem =>
                {
                    lineItem.NotEmpty().WithMessage(notEmptyMessage);
                    lineItem.MustBeEntity(lineItem =>
                        VendorInvoiceLineItem.Create(
                            lineItem.Type,
                            VendorInvoiceItem.Create(
                                lineItem.Item.PartNumber,
                                lineItem.Item.Description,
                                lineItem?.Item?.Manufacturer is null
                                ? null
                                : Manufacturer.Create(
                                    lineItem?.Item?.Manufacturer.Name,
                                    lineItem?.Item?.Manufacturer.Prefix,
                                    lineItem?.Item?.Manufacturer.Code)
                                .Value,
                                lineItem?.Item?.SaleCode is null
                                ? null
                                : SaleCode.Create(
                                    lineItem?.Item?.SaleCode.Name,
                                    lineItem?.Item?.SaleCode.Code,
                                    (double)(lineItem?.Item?.SaleCode.LaborRate),
                                    (double)(lineItem?.Item?.SaleCode.DesiredMargin),
                                    shopSupplies)
                                .Value)
                            .Value,
                            lineItem.Quantity,
                            lineItem.Cost,
                            lineItem.Core,
                            lineItem?.PONumber,
                            lineItem?.TransactionDate
                        ));
                });
        }
    }
}
