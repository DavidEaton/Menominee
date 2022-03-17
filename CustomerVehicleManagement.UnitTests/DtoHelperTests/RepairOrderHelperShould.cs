using CustomerVehicleManagement.Shared.Models.RepairOrders;
using FluentAssertions;
using System.IO;
using System.Text.Json;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.DtoHelperTests
{
    public class RepairOrderHelperShould
    {
        private RepairOrderToRead repairOrder;

        [Fact]
        public void Convert_Read_Dto_To_Write_Dto()
        {
            repairOrder = CreateRepairOrder();

            repairOrder.Should().NotBeNull();
        }

        private RepairOrderToRead CreateRepairOrder()
        {
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            RepairOrderToRead repairOrder = JsonSerializer.Deserialize<RepairOrderToRead>(jsonString)!;

            return repairOrder;
        }

    }
}
