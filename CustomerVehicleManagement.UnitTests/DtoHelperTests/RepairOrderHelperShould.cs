using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.DtoHelperTests
{
    public class RepairOrderHelperShould
    {
        private RepairOrderToRead repairOrder;

        [Fact]
        public void ConvertReadDtoToWriteDto()
        {
            // Arrange
            string jsonString = File.ReadAllText("./TestData/repair-order.json");
            repairOrder = CreateRepairOrder(jsonString);

            // Act
            var repairOrderToWrite = RepairOrderHelper.ConvertReadDtoToWriteDto(repairOrder);

            // Assert
            repairOrderToWrite.Should().BeOfType<RepairOrderToWrite>();
        }

        [Fact]
        public void Return_Correct_SerialNumbersMissingCount_On_ConvertReadDtoToWriteDto()
        {
            // repair-order-graph.json contains 1 Services row, having one Items row,
            // having "quantitySold": 5, and 4 SerialNumbers rows. Therefore, 
            // SerialNumbersMissingCount == 1.
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = CreateRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.ConvertReadDtoToWriteDto(repairOrder);

            var serialNumbers = repairOrderToEdit.Services[0].Items[0].SerialNumbers;
            int createdSerialNumbersCount = serialNumbers.Count(
                serialNumber =>
                string.IsNullOrWhiteSpace(serialNumber.SerialNumber));

            RepairOrderHelper.MissingSerialNumberCount(repairOrderToEdit.Services).Should().Be(createdSerialNumbersCount);
        }

        [Fact]
        public void Return_Correct_WarrantiesMissingCount_On_ConvertReadDtoToWriteDto()
        {
            // repair-order-graph.json contains 1 Services row, having one Items row,
            // having "quantitySold": 5, and 4 Warranties rows. Therefore, 
            // SerialNumbersMissingCount == 1.
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = CreateRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.ConvertReadDtoToWriteDto(repairOrder);

            var warranties = repairOrderToEdit.Services[0].Items[0].Warranties;
            int createdWarrantiesCount = warranties.Count(
                warranty =>
                warranty.SequenceNumber == 0);

            RepairOrderHelper.MissingWarrantyCount(repairOrderToEdit.Services).Should().Be(createdWarrantiesCount);
        }

        [Fact]
        public void Create_Missing_SerialNumbers_On_ConvertReadDtoToWriteDto()
        {
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = CreateRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.ConvertReadDtoToWriteDto(repairOrder);

            var createdSerialNumbers = repairOrderToEdit.Services[0].Items[0].SerialNumbers;

            createdSerialNumbers[0].Should().BeOfType<RepairOrderSerialNumberToWrite>();
        }

        [Fact]
        public void Calculate_ExistingSerialNumbersCount_On_ConvertReadDtoToWriteDto()
        {
            string jsonString = File.ReadAllText("./TestData/repair-order-graph.json");
            repairOrder = CreateRepairOrder(jsonString);
            var repairOrderToEdit = RepairOrderHelper.ConvertReadDtoToWriteDto(repairOrder);

            var createdSerialNumbers = repairOrderToEdit.Services[0].Items[0].SerialNumbers;
            int createdSerialNumbersCount = createdSerialNumbers.Count(
                serialNumber =>
                string.IsNullOrWhiteSpace(serialNumber.SerialNumber));
            int quantitySold = (int)repairOrderToEdit.Services[0].Items[0].QuantitySold;
            int existingSerialNumbersCount = quantitySold - createdSerialNumbersCount;

            createdSerialNumbersCount.Should().Be(RepairOrderHelper.MissingSerialNumberCount(repairOrderToEdit.Services));
            existingSerialNumbersCount.Should().Be(quantitySold - createdSerialNumbersCount);
        }

        private RepairOrderToRead CreateRepairOrder(string jsonString)
        {
            RepairOrderToRead repairOrder = JsonSerializer.Deserialize<RepairOrderToRead>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return repairOrder;
        }
    }
}
