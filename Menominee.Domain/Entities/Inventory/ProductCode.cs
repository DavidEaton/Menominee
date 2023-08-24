using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class ProductCode : Entity
    {
        public static readonly string NonuniqueMessage = $"Manufacturer/Code combination is already in use and must be unique. The same Code may be used by more than one Manufacturer.";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumNameLength = 255;
        public static readonly int MaximumCodeLength = 8;
        public static string InvalidLengthMessage(int minLength, int maxLength) => $"Value must be between {minLength} and {maxLength} characters.";

        public Manufacturer Manufacturer { get; private set; }
        public string Code { get; private set; }
        public SaleCode SaleCode { get; private set; }
        public string Name { get; private set; }

        private ProductCode(
            Manufacturer manufacturer,
            string code,
            string name,
            SaleCode saleCode = null)
        {
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
            code = (code ?? string.Empty).Trim().ToUpper();
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumNameLength || name.Length < MinimumLength)
                return Result.Failure<ProductCode>(InvalidLengthMessage(MinimumLength, MaximumNameLength));

            if (code.Length > MaximumCodeLength || code.Length < MinimumLength)
                return Result.Failure<ProductCode>(InvalidLengthMessage(MinimumLength, MaximumCodeLength));

            if (manufacturer is null)
                return Result.Failure<ProductCode>(RequiredMessage);

            if (manufacturerCodes.Contains($"{manufacturer.Id}{code}"))
                return Result.Failure<ProductCode>(NonuniqueMessage);

            return Result.Success(new ProductCode(manufacturer, code, name, saleCode));
        }

        public Result<string> SetName(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumNameLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, MaximumNameLength));

            return Result.Success(Name = name);
        }

        public Result<string> SetCode(string code, IReadOnlyList<string> manufacturerCodes)
        {
            code = (code ?? string.Empty).Trim().ToUpper();

            if (code.Length > MaximumCodeLength || code.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, MaximumCodeLength));

            if (manufacturerCodes.Contains($"{Manufacturer.Id}{code}"))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(Code = code);
        }

        public Result<Manufacturer> SetManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return Result.Failure<Manufacturer>(RequiredMessage);

            //if (manufacturerPrefix.Contains($"{manufacturer.Prefix}"))
            //    return Result.Failure<Manufacturer>(NonuniqueMessage);

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
        protected ProductCode() { }

        #endregion    
    }
}
