﻿using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Enums;
using System;
using System.Linq;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLineItem?
    public class RepairOrderItem : Entity
    {
        // TODO: RepairOrderItem should have additional detail in new classes like RepairOrderItemPart, RepairOrderItemLabor, etc., similar to InventoryItem.cs - Al
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MaximumLength = 255;
        public static readonly int MinimumLength = 1;
        public static readonly string InvalidLengthMessage = $"Must be within {MinimumLength} to {MaximumLength} character(s) in length.";
        public Manufacturer Manufacturer { get; private set; } //required
        public string PartNumber { get; private set; } //required
        public string Description { get; private set; } //required
        public SaleCode SaleCode { get; private set; } //required
        public ProductCode ProductCode { get; private set; } //required
        public PartType PartType { get; private set; } //required
        public RepairOrderItemPart Part { get; private set; } //optional
        public RepairOrderItemLabor Labor { get; private set; } //optional
        public double ExciseFeesTotal => Part?.ExciseFeesTotal ?? 0;
        private RepairOrderItem(
            Manufacturer manufacturer,
            string partNumber,
            string description,
            SaleCode saleCode,
            ProductCode productCode,
            PartType partType,
            RepairOrderItemPart part = null,
            RepairOrderItemLabor labor = null
        )
        {
            Manufacturer = manufacturer;
            PartNumber = partNumber;
            Description = description;
            SaleCode = saleCode;
            ProductCode = productCode;
            PartType = partType;
            Part = part;
            Labor = labor;
        }

        public static Result<RepairOrderItem> Create(
            Manufacturer manufacturer,
            string partNumber,
            string description,
            SaleCode saleCode,
            ProductCode productCode,
            PartType partType,
            RepairOrderItemPart part = null,
            RepairOrderItemLabor labor = null
        )
        {
            partNumber = (partNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || partNumber.Length < MinimumLength ||
                description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<RepairOrderItem>(InvalidLengthMessage);

            if (manufacturer is null || saleCode is null || productCode is null)
                return Result.Failure<RepairOrderItem>(RequiredMessage);

            if (!Enum.IsDefined(typeof(PartType), partType))
                return Result.Failure<RepairOrderItem>(RequiredMessage);

            // Enforce invariant: one and only one optional member
            var validOptionalMembersCount = new[]
            {
                part is not null,
                labor is not null
            }.Count(optionalMembersCount => optionalMembersCount is true) == 1;

            return !validOptionalMembersCount
                ? Result.Failure<RepairOrderItem>(RequiredMessage)
                : Result.Success(new RepairOrderItem(manufacturer, partNumber, description, saleCode, productCode, partType, part, labor));
        }

        public Result<Manufacturer> SetManufacturer(Manufacturer manufacturer)
        {
            return
                manufacturer is null
                ? Result.Failure<Manufacturer>(RequiredMessage)
                : Result.Success(Manufacturer = manufacturer);
        }

        public Result<string> SetPartNumber(string partNumber)
        {
            partNumber = (partNumber ?? string.Empty).Trim();

            if (partNumber.Length < MinimumLength ||
                partNumber.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(PartNumber = partNumber);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            return
                description.Length < MinimumLength || description.Length > MaximumLength
                ? Result.Failure<string>(InvalidLengthMessage)
                : Result.Success(Description = description);
        }

        public Result<SaleCode> SetSaleCode(SaleCode saleCode)
        {
            return
                saleCode is null
                ? Result.Failure<SaleCode>(RequiredMessage)
                : Result.Success(SaleCode = saleCode);
        }

        public Result<ProductCode> SetProductCode(ProductCode productCode)
        {
            return
                productCode is null
                ? Result.Failure<ProductCode>(RequiredMessage)
                : Result.Success(ProductCode = productCode);
        }

        public Result<PartType> SetPartType(PartType partType)
        {
            return
                !Enum.IsDefined(typeof(PartType), partType)
                ? Result.Failure<PartType>(RequiredMessage)
                : Result.Success(PartType = partType);
        }

        public Result<RepairOrderItemPart> SetPart(RepairOrderItemPart part)
        {
            return
                part is null
                ? Result.Failure<RepairOrderItemPart>(RequiredMessage)
                : Result.Success(Part = part);
        }

        public Result<RepairOrderItemLabor> SetLabor(RepairOrderItemLabor labor)
        {
            return
                labor is null
                ? Result.Failure<RepairOrderItemLabor>(RequiredMessage)
                : Result.Success(Labor = labor);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderItem() { }

        #endregion

    }
}
