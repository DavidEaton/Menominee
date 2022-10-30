using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class ProductCode : Entity
    {
        public static readonly string NonuniqueMessage = $"Manufacturer/Code combination is already in use and must be unique. The same Code may be used by more than one Manufacturer.";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidNameLengthMessage = $"Name must be between {MinimumLength} and {MaximumNameLength} characters";
        public static readonly string InvalidCodeLengthMessage = $"Code must be between {MinimumLength} and {MaximumCodeLength} characters";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumNameLength = 255;
        public static readonly int MaximumCodeLength = 10;

        public Manufacturer Manufacturer { get; private set; }
        public string Code { get; private set; }
        public SaleCode SaleCode { get; private set; }
        public string Name { get; private set; }

        private ProductCode(
            Manufacturer manufacturer,
            string code, 
            string name,
            IReadOnlyList<string> manufacturerCodes,
            SaleCode saleCode = null)
        {
            code = (code ?? string.Empty).Trim();
            name = (name ?? string.Empty).Trim();
            if (name.Length > MaximumNameLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidNameLengthMessage);

            if (code.Length > MaximumCodeLength || code.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidCodeLengthMessage);

            if (manufacturer is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            // TODO:
            // Manufacturer/Code pair must be unique. The same code value may be used by more than one Manufacturer.
            if (manufacturerCodes.Contains($"{manufacturer.Id} + {manufacturer.Code}"))
                throw new ArgumentOutOfRangeException(NonuniqueMessage);

            Manufacturer = manufacturer;
            Code = code;
            SaleCode = saleCode;
            Name = name;
        }

        public static Result<ProductCode> Create(
            Manufacturer manufacturer,
            string code, 
            string name,
            IReadOnlyList<string> manufacturerCodes,
            SaleCode saleCode = null)
        {
            code = (code ?? string.Empty).Trim();
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumNameLength || name.Length < MinimumLength)
                return Result.Failure<ProductCode>(InvalidNameLengthMessage);

            if (code.Length > MaximumCodeLength || code.Length < MinimumLength)
                return Result.Failure<ProductCode>(InvalidCodeLengthMessage);

            if (manufacturer is null)
                return Result.Failure<ProductCode>(RequiredMessage);

            // TODO:
            // Manufacturer/Code pair must be unique. The same code value may be used by more than one Manufacturer.
            if (manufacturerCodes.Contains($"{manufacturer.Id} + {manufacturer.Code}"))
                return Result.Failure<ProductCode>(NonuniqueMessage);

            return Result.Success(new ProductCode(manufacturer, code, name, manufacturerCodes, saleCode));
        }

        public Result<string> SetName(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumNameLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidNameLengthMessage);

            return Result.Success(Name = name);
        }

        public Result<string> SetCode(string code)
        {
            code = (code ?? string.Empty).Trim();

            if (code.Length > MaximumCodeLength || code.Length < MinimumLength)
                return Result.Failure<string>(InvalidCodeLengthMessage);

            var ManufacturerCode = $"{Manufacturer.Id} + {code}";
            var manufacturerCodes = new List<string>();

            if (manufacturerCodes.Contains(ManufacturerCode))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(Code = code);
        }

        public Result<Manufacturer> SetManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return Result.Failure<Manufacturer>(RequiredMessage);

            var ManufacturerCode = $"{manufacturer.Id} + {Code}";
            var manufacturerCodes = new List<string>();

            if (manufacturerCodes.Contains(ManufacturerCode))
                return Result.Failure<Manufacturer>(NonuniqueMessage);
            
            return Result.Success(Manufacturer = manufacturer);
        }

        public Result<SaleCode> SetSaleCode(SaleCode saleCode)
        {
            if (saleCode is null)
                return Result.Failure<SaleCode>(RequiredMessage);

            return Result.Success(SaleCode = saleCode);
        }

        #region ORM

        // EF requires a parameterless constructor
        public ProductCode() { }

        #endregion    
    }
}
