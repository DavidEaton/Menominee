﻿using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly int MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value must be > {MinimumValue}.";

        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public int Order { get; private set; }
        public InventoryItem Item { get; private set; }
        public InventoryItemPackageDetails InventoryItemPackageDetails { get; private set; }

        private InventoryItemPackageItem(InventoryItemPackage package, int order, InventoryItem item, InventoryItemPackageDetails details)
        {
            if (package is null || item is null || details is null)
                throw new ArgumentOutOfRangeException(RequiredMessage); 

            if (order < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            InventoryItemPackage = package;
            Order = order;
            Item = item;
            InventoryItemPackageDetails = details;
        }

        public static Result<InventoryItemPackageItem> Create(InventoryItemPackage package, int order, InventoryItem item, InventoryItemPackageDetails details)
        {
            if (package is null || item is null || details is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            if (order < MinimumValue)
                return Result.Failure<InventoryItemPackageItem>(MinimumValueMessage);

            return Result.Success(new InventoryItemPackageItem(package, order, item, details));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageItem() { }

        #endregion    
    }
}
