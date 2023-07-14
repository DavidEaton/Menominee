﻿using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities
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
            Name = name;
            Code = code;
            LaborRate = laborRate;
            DesiredMargin = desiredMargin;
            ShopSupplies = shopSupplies;  // By the time we get here, ShopSupplies validation has already occurred; no need to repeat here.
        }
        public static Result<SaleCode> Create(string name, string code, double laborRate, double desiredMargin, SaleCodeShopSupplies shopSupplies)
        {
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

        public Result<string> SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<string>(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Name = name);
        }

        public Result<string> SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure<string>(RequiredMessage);

            code = (code ?? string.Empty).Trim();

            if (code.Length > MaximumLength || code.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

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
            if (desiredMargin < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

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