using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemsValidator : AbstractValidator<IList<VendorInvoiceLineItemToWrite>>
    {
        private const string notEmptyMessage = "Line Item must not be empty.";

        public VendorInvoiceLineItemsValidator()
        {
            SaleCodeShopSupplies shopSupplies = new SaleCodeShopSupplies();

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
                                Manufacturer.Create(
                                    lineItem.Item.Manufacturer.Name,
                                    lineItem.Item.Manufacturer.Prefix,
                                    lineItem.Item.Manufacturer.Code)
                                .Value,
                                SaleCode.Create(
                                    lineItem.Item.SaleCode.Name,
                                    lineItem.Item.SaleCode.Code,
                                    lineItem.Item.SaleCode.LaborRate,
                                    lineItem.Item.SaleCode.DesiredMargin,
                                    shopSupplies)
                                .Value)
                            .Value,
                            lineItem.Quantity,
                            lineItem.Cost,
                            lineItem.Core,
                            lineItem.PONumber,
                            lineItem.TransactionDate
                        ));
                });
        }
    }
}
