using Menominee.Domain.Entities.Taxes;

namespace Menominee.Shared.Models.Taxes
{
    public class SalesTaxHelper
    {
        public static SalesTax ConvertWriteDtoToEntity(SalesTaxToWrite tax)
        {
            return tax is null
                ? null
                : SalesTax.Create(
                    tax.Description,
                    tax.TaxType,
                    tax.Order,
                    tax.TaxIdNumber,
                    tax.PartTaxRate,
                    tax.LaborTaxRate,
                    ExciseFeeHelper.ConvertWriteDtosToEntities(tax.ExciseFees),
                    tax.IsAppliedByDefault,
                    tax.IsTaxable)
                .Value;
        }

        public static SalesTaxToWrite ConvertReadToWriteDto(SalesTaxToRead taxToRead)
        {
            return taxToRead is null
                ? new()
                : new()
                {
                    Description = taxToRead.Description,
                    TaxType = taxToRead.TaxType,
                    Order = taxToRead.Order,
                    IsAppliedByDefault = taxToRead.IsAppliedByDefault,
                    IsTaxable = taxToRead.IsTaxable,
                    TaxIdNumber = taxToRead.TaxIdNumber,
                    PartTaxRate = taxToRead.PartTaxRate,
                    LaborTaxRate = taxToRead.LaborTaxRate,
                    ExciseFees = ExciseFeeHelper.ConvertReadToWriteDtos(taxToRead?.ExciseFees)
                };
        }

        public static SalesTaxToRead ConvertToReadDto(SalesTax salesTax)
        {
            return salesTax is null
                ? new()
                : new()
                {
                    Id = salesTax.Id,
                    Description = salesTax.Description,
                    TaxType = salesTax.TaxType,
                    Order = salesTax.Order,
                    IsAppliedByDefault = salesTax.IsAppliedByDefault,
                    IsTaxable = salesTax.IsTaxable,
                    TaxIdNumber = salesTax.TaxIdNumber,
                    PartTaxRate = salesTax.PartTaxRate,
                    LaborTaxRate = salesTax.LaborTaxRate,
                    ExciseFees = ExciseFeeHelper.ConvertToReadDtos(salesTax?.ExciseFees)
                };
        }

        public static SalesTaxToReadInList ConvertToReadInListDto(SalesTax tax)
        {
            return tax is null
                ? new()
                : new()
                {
                    Id = tax.Id,
                    Description = tax.Description,
                    TaxType = tax.TaxType,
                    Order = tax.Order,
                    IsAppliedByDefault = tax.IsAppliedByDefault,
                    IsTaxable = tax.IsTaxable,
                    TaxIdNumber = tax.TaxIdNumber,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate
                };
        }

        public static SalesTax ConvertReadDtoToEntity(SalesTaxToRead salesTax)
        {
            return SalesTax.Create(
                salesTax.Description,
                salesTax.TaxType,
                salesTax.Order,
                salesTax.TaxIdNumber,
                salesTax.PartTaxRate,
                salesTax.LaborTaxRate,
                ExciseFeeHelper.ConvertReadDtoToEntity(salesTax.ExciseFees))
            .Value;
        }
    }
}
