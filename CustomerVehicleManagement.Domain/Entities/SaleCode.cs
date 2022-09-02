using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";

        public string Name { get; private set; }
        public string Code { get; private set; }
        public double LaborRate { get; private set; }
        public double DesiredMargin { get; private set; }
        public SaleCodeShopSupplies ShopSupplies { get; private set; }

        // TODO - Should royalty be split out???
        private SaleCode(string name, string code, double laborRate, double desiredMargin, SaleCodeShopSupplies shopSupplies)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(code))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            code = (code ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength ||
                code.Length > MaximumLength || code.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            if (laborRate < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if (desiredMargin < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Name = name;
            Code = code;
            LaborRate = laborRate;
            DesiredMargin = desiredMargin;
            ShopSupplies = shopSupplies;  // By the time we get here, ShopSupplies validation has already occurred; no need to repeat here.
        }
        public static Result<SaleCode> Create(string name, string code, double laborRate, double desiredMargin, SaleCodeShopSupplies shopSupplies)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(code))
                return Result.Failure<SaleCode>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure<SaleCode>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            code = (code ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength ||
                code.Length > MaximumLength || code.Length < MinimumLength)
                return Result.Failure<SaleCode>(InvalidLengthMessage);

            if (laborRate < MinimumValue)
                return Result.Failure<SaleCode>(MinimumValueMessage);

            if (desiredMargin < MinimumValue)
                return Result.Failure<SaleCode>(MinimumValueMessage);

            if (shopSupplies is null)
                return Result.Failure<SaleCode>(RequiredMessage);

            return Result.Success(new SaleCode(name, code, laborRate, desiredMargin, shopSupplies));
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            code = (code ?? string.Empty).Trim();

            if (code.Length > MaximumLength || code.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Code = code;
        }

        public void SetLaborRate(double laborRate)
        {
            if (laborRate < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            LaborRate = laborRate;
        }

        public void SetDesiredMargin(double desiredMargin)
        {
            if (desiredMargin < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            DesiredMargin = desiredMargin;
        }

        public void SetShopSupplies(SaleCodeShopSupplies shopSupplies)
        {
            if (shopSupplies is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            ShopSupplies = shopSupplies;
        }


        #region ORM

        // EF requires an empty constructor
        public SaleCode() { }

        #endregion 
    }
}
