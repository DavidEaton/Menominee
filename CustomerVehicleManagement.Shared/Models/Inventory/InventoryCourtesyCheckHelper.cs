using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryCourtesyCheckHelper
    {
        public static InventoryCourtesyCheckToWrite ConvertReadToWriteDto(InventoryCourtesyCheckToRead courtesyCheck)
        {
            if (courtesyCheck is null)
                return null;

            return new()
            {
                LaborType = courtesyCheck.LaborType,
                LaborAmount = courtesyCheck.LaborAmount,
                TechPayType = courtesyCheck.TechPayType,
                TechPayAmount = courtesyCheck.TechPayAmount,
                SkillLevel = courtesyCheck.SkillLevel
            };
        }

        public static InventoryItemCourtesyCheck ConvertWriteDtoToEntity(InventoryCourtesyCheckToWrite courtesyCheck)
        {
            if (courtesyCheck is null)
                return null;

            return new()
            {
                LaborType = courtesyCheck.LaborType,
                LaborAmount = courtesyCheck.LaborAmount,
                TechPayType = courtesyCheck.TechPayType,
                TechPayAmount = courtesyCheck.TechPayAmount,
                SkillLevel = courtesyCheck.SkillLevel
            };
        }

        public static InventoryCourtesyCheckToRead ConvertEntityToReadDto(InventoryItemCourtesyCheck courtesyCheck)
        {
            if (courtesyCheck is null)
                return null;

            return new()
            {
                Id = courtesyCheck.Id,
                LaborType = courtesyCheck.LaborType,
                LaborAmount = courtesyCheck.LaborAmount,
                TechPayType = courtesyCheck.TechPayType,
                TechPayAmount = courtesyCheck.TechPayAmount,
                SkillLevel = courtesyCheck.SkillLevel
            };
        }

        public static void CopyWriteDtoToEntity(InventoryCourtesyCheckToWrite courtesyCheckToUpdate, InventoryItemCourtesyCheck courtesyCheck)
        {
            courtesyCheck.LaborType = courtesyCheckToUpdate.LaborType;
            courtesyCheck.LaborAmount = courtesyCheckToUpdate.LaborAmount;
            courtesyCheck.TechPayType = courtesyCheckToUpdate.TechPayType;
            courtesyCheck.TechPayAmount = courtesyCheckToUpdate.TechPayAmount;
            courtesyCheck.SkillLevel = courtesyCheckToUpdate.SkillLevel;
        }

        public static InventoryCourtesyCheckToReadInList ConvertEntityToReadInListDto(InventoryItemCourtesyCheck courtesyCheck)
        {
            if (courtesyCheck is null)
                return null;

            return new()
            {
                LaborType = courtesyCheck.LaborType,
                LaborAmount = courtesyCheck.LaborAmount,
                TechPayType = courtesyCheck.TechPayType,
                TechPayAmount = courtesyCheck.TechPayAmount,
                SkillLevel = courtesyCheck.SkillLevel
            };
        }
    }
}
