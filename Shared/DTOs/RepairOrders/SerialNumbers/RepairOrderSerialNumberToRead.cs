
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.SerialNumbers
{
    public class RepairOrderSerialNumberToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public string SerialNumber { get; set; }
        
        public static IReadOnlyList<RepairOrderSerialNumberToRead> ConvertToDto(IList<RepairOrderSerialNumber> serialNumbers)
        {
            return serialNumbers
                .Select(serialNumber =>
                        ConvertToDto(serialNumber))
                .ToList();
        }

        private static RepairOrderSerialNumberToRead ConvertToDto(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber != null)
            {
                return new RepairOrderSerialNumberToRead()
                {
                    Id = serialNumber.Id,
                    RepairOrderItemId = serialNumber.RepairOrderItemId,
                    SerialNumber = serialNumber.SerialNumber
                };
            }

            return null;
        }
    }
}
