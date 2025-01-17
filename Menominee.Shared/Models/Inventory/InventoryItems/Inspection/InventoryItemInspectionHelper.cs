﻿using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Inspection
{
    public class InventoryItemInspectionHelper
    {
        public static InventoryItemInspectionToWrite ConvertReadToWriteDto(InventoryItemInspectionToRead inspection)
        {
            return inspection is null
                ? null
                : new()
                {
                    LaborAmount = new LaborAmountToWrite()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.PayType,
                    },
                    TechAmount = new TechAmountToWrite()
                    {
                        PayType = inspection.TechAmount.PayType,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.Type
                };
        }

        public static InventoryItemInspection ConvertWriteDtoToEntity(InventoryItemInspectionToWrite inspection)
        {
            return inspection is null
                ? null
                : InventoryItemInspection.Create(
                LaborAmount.Create(
                    inspection.LaborAmount.PayType,
                    inspection.LaborAmount.Amount).
                Value,
                TechAmount.Create(
                    inspection.TechAmount.PayType,
                    inspection.TechAmount.Amount,
                    inspection.TechAmount.SkillLevel)
                .Value,
                inspection.Type).Value;
        }

        public static InventoryItemInspectionToRead ConvertToReadDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : new()
                {
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.Type,
                    },
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = inspection.TechAmount.Type,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.InspectionType,
                };
        }

        public static InventoryItemInspectionToReadInList ConvertToReadInListDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : new()
                {
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.Type,
                    },
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = inspection.TechAmount.Type,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.InspectionType,
                };
        }

        public static InventoryItemInspectionToWrite ConvertToWriteDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : new()
                {
                    Id = inspection.Id,
                    LaborAmount = new LaborAmountToWrite()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.Type,
                    },
                    TechAmount = new TechAmountToWrite()
                    {
                        PayType = inspection.TechAmount.Type,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.InspectionType
                };
        }
    }
}
