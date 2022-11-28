using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxHelper
    {

        public static List<VendorInvoiceTax> ConvertWriteDtosToEntities(IList<VendorInvoiceTaxToWrite> taxes, IReadOnlyList<SalesTax> salesTaxes)
        {
            return taxes?.Select(ConvertWriteDtoToEntity(salesTaxes)).ToList()
                ?? new List<VendorInvoiceTax>();
        }

        public static Func<VendorInvoiceTaxToWrite, VendorInvoiceTax> ConvertWriteDtoToEntity(IReadOnlyList<SalesTax> salesTaxes)
        {
            return tax =>
                VendorInvoiceTax.Create(salesTaxes.FirstOrDefault(
                    salesTax =>
                    salesTax.Id == tax.SalesTax.Id),
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
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = tax.SalesTax
                };
        }
    }
}
