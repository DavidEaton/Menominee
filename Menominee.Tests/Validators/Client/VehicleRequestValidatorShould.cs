using FluentAssertions;
using Menominee.Client.Components.Vehicles;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary;
using System.Linq;
using Xunit;

namespace Menominee.Tests.Validators.Client
{
    public class VehicleRequestValidatorShould
    {
        private readonly VehicleRequestValidator validator;
        private const int FutureYear = 2050; // A year well in the future

        public VehicleRequestValidatorShould()
        {
            validator = new VehicleRequestValidator();
        }

        [Fact]
        public void ValidateAndGetErrors()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];

            var errorMessages = validator.ValidateAndGetErrors(vehicleRequest);

            errorMessages.Should().BeEmpty("because all vehicle details are valid.");
        }

        [Fact]
        public void Validate_With_Valid_Vehicle_Details()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_When_NonTraditionalVehicle_With_Model_Only()
        {
            var vehicleRequest = new VehicleToWrite
            {
                Model = "F150",
                NonTraditionalVehicle = true
            };

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_When_NonTraditionalVehicle_With_Make_Only()
        {
            var vehicleRequest = new VehicleToWrite
            {
                Make = "Ford",
                NonTraditionalVehicle = true
            };

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Vehicle_Details()
        {
            var vehicleRequest = new VehicleToWrite() { };

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_VIN()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.VIN = "123456789012345679999";

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Make()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.Make = string.Empty;

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains(Vehicle.InvalidLengthMessage)).Should().BeTrue();
        }

        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ")]
        [InlineData("")]
        [InlineData("A")]
        //[InlineData("                   ")] // This should be trimmed but validators can't mutate the model; client code should trim before sending requests
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit.Lorem ipsum dolor sit amet, consectetuer adipiscing elit.Lorem ipsum dolor sit amet, consectetuer adipiscing elit.")]
        public void Not_Validate_With_Invalid_Model(string model)
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.Model = model;

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ")]
        public void Not_Validate_With_Invalid_Plate(string plate)
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.Plate = plate;

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumPlateLength))).Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Invalid_PlateStateProvince()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.PlateStateProvince = (State)999;

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains(Vehicle.InvalidPlateStateProvinceMessage)).Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Invalid_UnitNumber()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.UnitNumber = new string('A', Vehicle.MaximumUnitNumberLength + 1);

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumUnitNumberLength))).Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Color()
        {
            var vehicleRequest = TestDataFactory.CreateVehiclesList()[0];
            vehicleRequest.Color = new string('A', Vehicle.MaximumColorLength + 1);

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumColorLength))).Should().BeTrue();
        }

        [Fact]
        public void Validate_With_Null_Year()
        {
            var vehicleRequest = new VehicleToWrite
            {
                VIN = "12345678901234567",
                Year = null,
                Make = "Ford",
                Model = "F150",
                Plate = "ABC123",
                PlateStateProvince = State.AK,
                UnitNumber = "123",
                Color = "Red",
                Active = true,
                NonTraditionalVehicle = false
            };

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(Vehicle.YearMinimum - 1)]
        [InlineData(FutureYear)]
        public void Not_Validate_With_Invalid_Year(int? year)
        {
            var vehicleRequest = new VehicleToWrite
            {
                VIN = "12345678901234567",
                Year = year,
                Make = "Ford",
                Model = "F150",
                Plate = "ABC123",
                PlateStateProvince = State.AK,
                UnitNumber = "123",
                Color = "Red",
                Active = true,
                NonTraditionalVehicle = false
            };

            var result = validator.Validate(vehicleRequest);

            result.IsValid.Should().BeFalse("because the year is not within the valid range");
            if (year is not null)
            {
                result.Errors.Should().ContainSingle(e => e.ErrorMessage.Contains(Vehicle.InvalidYearMessage), "because the year is outside the valid range");
            }
        }
    }

}
