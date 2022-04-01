using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.DtoHelperTests
{
    public class RepairOrderHelperShould
    {
        private RepairOrderToRead repairOrder;

        [Fact]
        public void CreateRepairOrder()
        {
            // Arrange
            string jsonString = File.ReadAllText("./TestData/repair-order.json");
            repairOrder = DeserializeRepairOrder(jsonString);

            // Act
            var repairOrderToWrite = RepairOrderHelper.CreateRepairOrder(repairOrder);

            // Assert
            repairOrderToWrite.Should().BeOfType<RepairOrderToWrite>();
        }

        [Fact]
        public void Return_Correct_SerialNumberRequiredMissingCount()
        {
            // repair-order-graph.json contains 1 Services row, having one Items row,
            // having "quantitySold": 5, and 4 SerialNumbers rows. Therefore, 
            // SerialNumbersMissingCount == 1.
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = DeserializeRepairOrder(jsonString);

            var repairOrderToEdit = RepairOrderHelper.CreateRepairOrder(repairOrder);

            RepairOrderHelper.SerialNumberRequiredMissingCount(repairOrderToEdit.Services).Should().Be(1);
        }

        [Fact]
        public void BuildSerialNumberList()
        {
            // repair-order-graph.json contains 1 Services row, having one Items row,
            // having "quantitySold": 5, and 4 SerialNumbers rows. Therefore, 
            // SerialNumberList.Count == 5 (4 existing plus 1 new, empty row for user to complete).

            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = DeserializeRepairOrder(jsonString);

            var repairOrderToEdit = RepairOrderHelper.CreateRepairOrder(repairOrder);
            var serialNumberList = RepairOrderHelper.BuildSerialNumberList(repairOrderToEdit.Services);

            serialNumberList.Should().BeOfType<List<SerialNumberListItem>>();
            serialNumberList.Count.Should().Be(5);
        }

        [Fact]
        public void Return_Correct_WarrantyRequiredMissingCount()
        {
            // repair-order-graph.json contains 1 Services row, having one Items row,
            // having "quantitySold": 5, and 1 Warranties row. Therefore, 
            // SerialNumbersMissingCount == 4.
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = DeserializeRepairOrder(jsonString);

            var repairOrderToEdit = RepairOrderHelper.CreateRepairOrder(repairOrder);

            RepairOrderHelper.WarrantyRequiredMissingCount(repairOrderToEdit.Services).Should().Be(4);
        }

        [Fact]
        public void Create_Missing_SerialNumbers_On_CreateRepairOrder()
        {
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = DeserializeRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.CreateRepairOrder(repairOrder);

            var createdSerialNumbers = repairOrderToEdit.Services[0].Items[0].SerialNumbers;

            createdSerialNumbers[0].Should().BeOfType<RepairOrderSerialNumberToWrite>();
        }

        [Fact]
        public void Create_Missing_Warranties_On_CreateRepairOrder()
        {
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = DeserializeRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.CreateRepairOrder(repairOrder);

            var createdWarranties = repairOrderToEdit.Services[0].Items[0].Warranties;

            createdWarranties[0].Should().BeOfType<RepairOrderWarrantyToWrite>();
        }

        private RepairOrderToRead DeserializeRepairOrder(string jsonString)
        {
            RepairOrderToRead repairOrder = JsonSerializer.Deserialize<RepairOrderToRead>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return repairOrder;
        }
    }
}
