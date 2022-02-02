using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
{
    public class RepairOrderTechToRead
    {
        public long Id { get; set; }
        public long RepairOrderServiceId { get; set; }
        public long TechnicianId { get; set; }

        public static IReadOnlyList<RepairOrderTechToRead> ConvertToDto(IList<RepairOrderTech> techs)
        {
            return techs
                .Select(tech =>
                        ConvertToDto(tech))
                .ToList();
        }

        private static RepairOrderTechToRead ConvertToDto(RepairOrderTech tech)
        {
            if (tech != null)
            {
                return new RepairOrderTechToRead()
                {
                    Id = tech.Id,
                    RepairOrderServiceId = tech.RepairOrderServiceId,
                    TechnicianId = tech.TechnicianId
                };
            }

            return null;
        }
    }
}
