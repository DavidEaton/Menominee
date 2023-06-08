using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxHelper
    {
        public static IReadOnlyList<VendorInvoiceTax> ConvertWriteDtosToEntities(IList<VendorInvoiceTaxToWrite> taxes)
        {
            return taxes.Select(tax =>
                VendorInvoiceTax.Create(
                    SalesTaxHelper.ConvertReadDtoToEntity(tax.SalesTax),
                    tax.Amount).Value
            ).ToList();
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

        internal static SalesTaxToRead ConvertToReadDto(SalesTax tax)
        {
            return tax is null
                ? null
                : new()
                {
                    Description = tax.Description,
                    Id = tax.Id,
                    IsAppliedByDefault = tax.IsAppliedByDefault,
                    IsTaxable = tax.IsTaxable,
                    LaborTaxRate = tax.LaborTaxRate,
                    Order = tax.Order,
                    PartTaxRate = tax.PartTaxRate,
                    ExciseFees = ExciseFeeHelper.ConvertToReadDtos(tax.ExciseFees),
                    TaxIdNumber = tax.TaxIdNumber,
                    TaxType = tax.TaxType
                };
        }

        public static IReadOnlyList<VendorInvoiceTaxToRead> ConvertTaxesToReadDtos(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            return taxes?.Select(ConvertTaxToReadDto()).ToList()
                ?? new List<VendorInvoiceTaxToRead>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToRead> ConvertTaxToReadDto()
        {
            return tax =>
                new()
                {
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = ConvertSalesTaxToReadDto(tax.SalesTax)
                };
        }

        private static SalesTaxToRead ConvertSalesTaxToReadDto(SalesTax salesTax)
        {
            return salesTax is null
                ? null
                : new()
                {
                    Description = salesTax.Description,
                    Id = salesTax.Id,
                    IsAppliedByDefault = salesTax.IsAppliedByDefault,
                    IsTaxable = salesTax.IsTaxable,
                    LaborTaxRate = salesTax.LaborTaxRate,
                    Order = salesTax.Order,
                    PartTaxRate = salesTax.PartTaxRate,
                    ExciseFees = ExciseFeeHelper.ConvertToReadDtos(salesTax.ExciseFees),
                    TaxIdNumber = salesTax.TaxIdNumber,
                    TaxType = salesTax.TaxType
                };
        }

        public static IList<VendorInvoiceTaxToWrite> ConvertTaxesToWriteDtos(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            return
                taxes is null
                ? null
                : taxes?.Select(tax =>
                    new VendorInvoiceTaxToWrite()
                    {
                        Id = tax.Id,
                        SalesTax = VendorInvoiceTaxHelper.ConvertToReadDto(tax.SalesTax),
                        Amount = tax.Amount
                    }).ToList()
                ?? new List<VendorInvoiceTaxToWrite>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToWrite> ConvertTaxToWriteDto()
        {
            return tax =>
                new()
                {
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = ConvertSalesTaxToReadDto(tax.SalesTax)
                };
        }
    }
}
