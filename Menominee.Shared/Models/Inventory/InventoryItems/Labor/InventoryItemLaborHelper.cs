﻿using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborHelper
    {
        public static InventoryItemLaborToWrite ConvertReadToWriteDto(InventoryItemLaborToRead labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                LaborAmount = new LaborAmountToWrite()
                {
                    Amount = labor.LaborAmount.Amount,
                    PayType = labor.LaborAmount.PayType,
                },
                TechAmount = new TechAmountToWrite()
                {
                    PayType = labor.TechAmount.PayType,
                    Amount = labor.TechAmount.Amount,
                    SkillLevel = labor.TechAmount.SkillLevel
                }
            };
        }

        public static InventoryItemLabor ConvertWriteDtoToEntity(InventoryItemLaborToWrite labor)
        {
            return labor is null
                ? null
                : InventoryItemLabor.Create(
                LaborAmount.Create(labor.LaborAmount.PayType, labor.LaborAmount.Amount).Value,
                TechAmount.Create(labor.TechAmount.PayType, labor.TechAmount.Amount, labor.TechAmount.SkillLevel).Value)
            .Value;
        }

        public static InventoryItemLaborToRead ConvertToReadDto(InventoryItemLabor labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                Id = labor.Id,
                LaborAmount = new LaborAmountToRead()
                {
                    Amount = labor.LaborAmount.Amount,
                    PayType = labor.LaborAmount.Type,
                },
                TechAmount = new TechAmountToRead()
                {
                    PayType = labor.TechAmount.Type,
                    Amount = labor.TechAmount.Amount,
                    SkillLevel = labor.TechAmount.SkillLevel
                }
            };
        }

        public static InventoryItemLaborToReadInList ConvertToReadInListDto(InventoryItemLabor labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                LaborAmount = new LaborAmountToRead()
                {
                    Amount = labor.LaborAmount.Amount,
                    PayType = labor.LaborAmount.Type,
                },
                TechAmount = new TechAmountToRead()
                {
                    PayType = labor.TechAmount.Type,
                    Amount = labor.TechAmount.Amount,
                    SkillLevel = labor.TechAmount.SkillLevel
                }
            };
        }

        internal static InventoryItemLaborToWrite ConvertToWriteDto(InventoryItemLabor labor)
        {
            return labor is null
                ? new()
                {
                    LaborAmount = new LaborAmountToWrite()
                    {
                        Amount = labor.LaborAmount.Amount,
                        PayType = labor.LaborAmount.Type,
                    },
                    TechAmount = new TechAmountToWrite()
                    {
                        PayType = labor.TechAmount.Type,
                        Amount = labor.TechAmount.Amount,
                        SkillLevel = labor.TechAmount.SkillLevel
                    }
                }
                : new();
        }
    }
}
