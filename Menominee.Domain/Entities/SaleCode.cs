using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities
{
    public class SaleCode : Entity
    {
        public static readonly int MinimumLength = 1;
        public static readonly int NameMaximumLength = 255;
        public static readonly int CodeMaximumLength = 4;
        public static readonly double MinimumValue = 0;
        public static readonly double MaximumDesiredMarginValue = 100;

        public static string InvalidLengthMessage(int minLength, int maxLength) => $"Value must be between {minLength} and {maxLength} characters.";
        public static string InvalidValueMessage(double minValue, double maxValue) => $"Value must be between {minValue} and {maxValue}.";
       
        public static readonly string RequiredMessage = $"Please include all required items."; 
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";
        public static readonly string NonuniqueMessage = $"Code is already in use and must be unique.";

        public string Name { get; private set; }
        public string Code { get; private set; }
        public double LaborRate { get; private set; }
        public double DesiredMargin { get; private set; }
        public SaleCodeShopSupplies ShopSupplies { get; private set; }

        // TODO - Should royalty be split out???
        private SaleCode(string name, string code, double laborRate, double desiredMargin, SaleCodeShopSupplies shopSupplies)
        {
            Name = name;
            Code = code;
            LaborRate = laborRate;
            DesiredMargin = desiredMargin;
            ShopSupplies = shopSupplies;  // By the time we get here, ShopSupplies validation has already occurred; no need to repeat here.
        }
        public static Result<SaleCode> Create(string name, string code, double laborRate, double desiredMargin, SaleCodeShopSupplies shopSupplies, IReadOnlyList<string> saleCodes)
        {
            name = (name ?? string.Empty).Trim();
            code = (code ?? string.Empty).Trim().ToUpper();

            if (name.Length > NameMaximumLength || name.Length < MinimumLength)
                return Result.Failure<SaleCode>(InvalidLengthMessage(MinimumLength, NameMaximumLength));

            if (code.Length > CodeMaximumLength || code.Length < MinimumLength)
                return Result.Failure<SaleCode>(InvalidLengthMessage(MinimumLength, CodeMaximumLength));

            if (laborRate < MinimumValue)
                return Result.Failure<SaleCode>(MinimumValueMessage);

            if (desiredMargin < MinimumValue || desiredMargin > MaximumDesiredMarginValue)
                return Result.Failure<SaleCode>(InvalidValueMessage(MinimumValue, MaximumDesiredMarginValue));

            if (shopSupplies is null)
                return Result.Failure<SaleCode>(RequiredMessage);

            if (saleCodes.Contains(code, StringComparer.OrdinalIgnoreCase))
                return Result.Failure<SaleCode>(NonuniqueMessage);

            return Result.Success(new SaleCode(name, code, laborRate, desiredMargin, shopSupplies));
        }

        public Result<string> SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<string>(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > NameMaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, NameMaximumLength));

            return Result.Success(Name = name);
        }

        public Result<string> SetCode(string code, IReadOnlyList<string> saleCodes)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure<string>(RequiredMessage);

            code = (code ?? string.Empty).Trim().ToUpper();

            if (code.Length > CodeMaximumLength || code.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, CodeMaximumLength));

            if (saleCodes.Contains(code, StringComparer.OrdinalIgnoreCase))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(Code = code);
        }

        public Result<double> SetLaborRate(double laborRate)
        {
            if (laborRate < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(LaborRate = laborRate);
        }

        public Result<double> SetDesiredMargin(double desiredMargin)
        {
            if (desiredMargin < MinimumValue || desiredMargin > MaximumDesiredMarginValue)
                return Result.Failure<double>(InvalidValueMessage(MinimumValue, MaximumDesiredMarginValue));

            return Result.Success(DesiredMargin = desiredMargin);
        }

        public Result<SaleCodeShopSupplies> SetShopSupplies(SaleCodeShopSupplies shopSupplies)
        {
            if (shopSupplies is null)
                return Result.Failure<SaleCodeShopSupplies>(MinimumValueMessage);

            return Result.Success(ShopSupplies = shopSupplies);
        }


        #region ORM

        // EF requires a parameterless constructor
        protected SaleCode() { }

        #endregion 
    }
}
