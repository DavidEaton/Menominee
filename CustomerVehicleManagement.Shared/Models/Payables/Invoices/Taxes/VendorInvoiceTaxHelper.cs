using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxHelper
    {

        public static IList<VendorInvoiceTax> ConvertWriteDtosToEntities(IList<VendorInvoiceTaxToWrite> taxes)
        {
            return taxes?.Select(ConvertWriteDtoToEntity()).ToList()
                ?? new List<VendorInvoiceTax>();
        }

        public static Func<VendorInvoiceTaxToWrite, VendorInvoiceTax> ConvertWriteDtoToEntity()
        {
            return tax =>
                VendorInvoiceTax.Create(
                    SalesTaxHelper.CreateSalesTax(tax.SalesTax),
                    tax.Order,
                    tax.TaxName,
                    tax.Amount)
                .Value;
        }

        public static IList<VendorInvoiceTaxToWrite> ConvertReadDtosToWriteDtos(IReadOnlyList<VendorInvoiceTaxToRead> taxes)
        {
            return taxes?.Select(ConvertReadDtoToWriteDto()).ToList()
                ?? new List<VendorInvoiceTaxToWrite>();
        }

        private static Func<VendorInvoiceTaxToRead, VendorInvoiceTaxToWrite> ConvertReadDtoToWriteDto()
        {
            return tax =>
                new VendorInvoiceTaxToWrite()
                {
                    SalesTax = SalesTaxHelper.CreateSalesTax(tax.SalesTax),
                    Order = tax.Order,
                    TaxId = tax.TaxId,
                    TaxName = tax.TaxName,
                    Amount = tax.Amount
                };
        }
    }
}
