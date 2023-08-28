using Menominee.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Menominee.Common.Enums;

namespace Menominee.Tests.Entities
{
    public class VehicleShould
    {
        [Fact]
        public void Create_Vehicle()
        {
            // Arrange
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            // Act
            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var vehicle = result.Value;
            vehicle.VIN.Should().Be(vin);
            vehicle.Year.Should().Be(year);
            vehicle.Make.Should().Be(make);
            vehicle.Model.Should().Be(model);
        }

        [Theory]
        [InlineData("1A4GJ45R92J214567", 2010, "Mid Michigan", "Trailer", "", null, "", "")]
        [InlineData("1A4GJ45R92J214567", null, "Mid Michigan", "", "", null, "", "")]
        [InlineData("1A4GJ45R92J214567", null, "", "Trailer", "", null, "", "")]
        [InlineData(null, null, "", "Trailer", "", null, "", "")]
        [InlineData(null, null, null, "Trailer", "", null, "", "")]
        public void Create_NonTraditional_Vehicle(string vin, int? year, string make, string model, string plate, State? plateStateProvince, string unitNumber, string color)
        {
            var active = true;
            var nonTraditionalVehicle = true;
            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Vehicle>();
        }

        [Theory]
        [InlineData(null, null, null, null, null, null, null, null)]
        [InlineData("1A4GJ45R92J214567", null, "", "", "", null, "", "")]
        [InlineData("1A4GJ45R92J214567", 2010, "", "", "", null, "", "")]
        public void Not_Create_Invalid_NonTraditional_Vehicle(string vin, int? year, string make, string model, string plate, State? plateStateProvince, string unitNumber, string color)
        {
            var active = true;
            var nonTraditionalVehicle = true;

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.NonTraditionalVehicleInvalidMakeModelMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.ValidYears), MemberType = typeof(TestData))]
        public void Create_Vehicle_With_Valid_Edge_Case_Years(int validYear)
        {
            var vin = "1A4GJ45R92J214567";
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, validYear, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(validYear);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1A4GJ45R92J21456")]
        [InlineData("5XYKUCA13BG0015")] // Only 16 characters long; too short
        [InlineData("5XYKUCA13BG0015178")] // Too long
        public void Not_Create_Vehicle_With_Invalid_Vin(string invalidVin)
        {
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(invalidVin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidVinMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidModels), MemberType = typeof(TestData))]
        public void Not_Create_Vehicle_With_Invalid_Model(string invalidModel)
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var nonTraditionalVehicle = false;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, make, invalidModel, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidLengthMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidModels), MemberType = typeof(TestData))]
        public void Create_NonTraditionalVehicle_With_NonTraditional_Model(string nonTraditionalModel)
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var nonTraditionalVehicle = true;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, make, nonTraditionalModel, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(year);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidModels), MemberType = typeof(TestData))]
        public void Create_NonTraditionalVehicle_With_NonTraditional_Make(string nonTraditionalMake)
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var model = "Pilot";
            var nonTraditionalVehicle = true;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, nonTraditionalMake, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(year);
        }

        [Fact]
        public void Create_NonTraditionalVehicle_With_Null_Vin()
        {
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var nonTraditionalVehicle = true;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(null, year, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(year);
        }

        [Fact]
        public void Not_Create_NonTraditionalVehicle_With_Invalid_Vin()
        {
            var nonTraditionalVin = "moops";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var nonTraditionalVehicle = true;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(nonTraditionalVin, year, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidVinMessage);
        }

        [Fact]
        public void Create_NonTraditionalVehicle_With_Null_Year()
        {
            var vin = "1A4GJ45R92J214567";
            int? nonTraditionalYear = null;
            var make = "Honda";
            var model = "Pilot";
            var nonTraditionalVehicle = true;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, nonTraditionalYear, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(nonTraditionalYear);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidMakes), MemberType = typeof(TestData))]
        public void Not_Create_Vehicle_With_Invalid_Make(string invalidMake)
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var model = "Pilot";
            var nonTraditionalVehicle = false;
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, invalidMake, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidLengthMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidYears), MemberType = typeof(TestData))]
        public void Not_Create_Vehicle_With_Invalid_Year(int invalidYear)
        {
            var vin = "1A4GJ45R92J214567";
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, invalidYear, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidYearMessage);
        }

        [Fact]
        public void Not_Create_Vehicle_With_Invalid_Plate()
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = Utilities.RandomCharacters(Vehicle.MaximumPlateLength + 1);
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumPlateLength));
        }

        [Fact]
        public void Not_Create_Vehicle_With_Invalid_PlateStateProvince()
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = (State)(-1);
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidPlateStateProvinceMessage);
        }

        [Fact]
        public void Not_Create_Vehicle_With_Invalid_UnitNumber()
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = Utilities.RandomCharacters(Vehicle.MaximumUnitNumberLength + 1);
            var color = "Blue";
            var active = true;

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumUnitNumberLength));
        }

        [Fact]
        public void Not_Create_Vehicle_With_Invalid_Color()
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = Utilities.RandomCharacters(Vehicle.MaximumUnitNumberLength + 1);
            var active = true;

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumColorLength));
        }

        [Fact]
        public void SetVin()
        {
            var vehicle = CreateVehicle();
            var newVin = "1A4GJ45R92J214568";
            vehicle.VIN.Should().NotBe(newVin);

            var result = vehicle.SetVin(newVin);

            result.IsSuccess.Should().BeTrue();
            vehicle.VIN.Should().Be(newVin);
        }

        [Fact]
        public void SetVin_With_Null()
        {
            var vehicle = CreateVehicle();
            var newVin = (string)null;
            vehicle.VIN.Should().NotBe(newVin);

            var result = vehicle.SetVin(newVin);

            result.IsSuccess.Should().BeTrue();
            vehicle.VIN.Should().Be(newVin);
        }

        [Fact]
        public void Not_Set_Invalid_Vin()
        {
            var vehicle = CreateVehicle();
            var newVin = "moops";
            vehicle.VIN.Should().NotBe(newVin);

            var result = vehicle.SetVin(newVin);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidVinMessage);
        }

        [Fact]
        public void Not_Set_Empty_Vin()
        {
            var vehicle = CreateVehicle();
            var newVin = string.Empty;
            vehicle.VIN.Should().NotBe(newVin);

            var result = vehicle.SetVin(newVin);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidVinMessage);
        }

        [Fact]
        public void SetYear()
        {
            var vehicle = CreateVehicle();
            var newYear = vehicle.Year - 1;
            vehicle.Year.Should().NotBe(newYear);

            var result = vehicle.SetYear(newYear);

            result.IsSuccess.Should().BeTrue();
            vehicle.Year.Should().Be(newYear);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidYears), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Year(int invalidYear)
        {
            var vehicle = CreateVehicle();

            var result = vehicle.SetYear(invalidYear);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidYearMessage);
        }

        [Fact]
        public void SetMake()
        {
            var vehicle = CreateVehicle();
            var newMake = "Toyota";
            vehicle.Make.Should().NotBe(newMake);

            var result = vehicle.SetMake(newMake);

            result.IsSuccess.Should().BeTrue();
            vehicle.Make.Should().Be(newMake);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidMakes), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Make(string invalidMake)
        {
            var vehicle = CreateVehicle();
            var originalMake = vehicle.Make;

            var result = vehicle.SetMake(invalidMake);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidLengthMessage);
            vehicle.Make.Should().Be(originalMake);
        }

        [Fact]
        public void SetModel()
        {
            var vehicle = CreateVehicle();
            var newModel = "Accord";
            vehicle.Model.Should().NotBe(newModel);

            var result = vehicle.SetModel(newModel);

            result.IsSuccess.Should().BeTrue();
            vehicle.Model.Should().Be(newModel);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidModels), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Model(string invalidModel)
        {
            var vehicle = CreateVehicle();

            var result = vehicle.SetModel(invalidModel);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidLengthMessage);
        }

        [Fact]
        public void Not_Set_Invalid_Plate()
        {
            var vehicle = CreateVehicle();
            var invalidPlate = Utilities.RandomCharacters(Vehicle.MaximumPlateLength + 1);

            var result = vehicle.SetPlate(invalidPlate);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumPlateLength));
        }

        [Fact]
        public void Not_Set_Invalid_PlateStateProvince()
        {
            var vehicle = CreateVehicle();
            var invalidPlateStateProvince = (State)(-1);

            var result = vehicle.SetPlateStateProvince(invalidPlateStateProvince);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidPlateStateProvinceMessage);
        }

        [Fact]
        public void Not_Set_Invalid_UnitNumber()
        {
            var vehicle = CreateVehicle();
            var invalidUnitNumber = Utilities.RandomCharacters(Vehicle.MaximumUnitNumberLength + 1);

            var result = vehicle.SetUnitNumber(invalidUnitNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumUnitNumberLength));
        }

        [Fact]
        public void Not_Set_Invalid_Color()
        {
            var vehicle = CreateVehicle();
            var invalidColor = Utilities.RandomCharacters(Vehicle.MaximumColorLength + 1);

            var result = vehicle.SetColor(invalidColor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumColorLength));
        }

        [Theory]
        [InlineData("1HGCM82633A123456", 2003, "Ho", "Ac", "", null, "", "", true, false, "2003 Ho Ac")]
        [InlineData("1HGCM82633A123456", null, "Ho", "Ac", "", null, "", "", true, false, "0 Ho Ac")]
        [InlineData("1HGCM82633A123456", 2003, "Honda", "Accord", "", null, "", "", true, false, "2003 Honda Accord")]
        [InlineData("1HGCM82633A123456", 2003, "Trailer", null, "", null, "", "", true, true, "2003 Trailer ")]
        [InlineData("1HGCM82633A123456", null, null, "Trailer", "", null, "", "", true, true, "0  Trailer")]
        [InlineData("1HGCM82633A123456", 2003, null, "Trailer", "", null, "", "", true, true, "2003  Trailer")]
        public void Format_ToString_Correctly(string vin, int? year, string make, string model, string plate, State? plateStateProvince, string unitNumber, string color, bool active, bool nonTraditionalVehicle, string expectedOutput)
        {
            var vehicleResult = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active, nonTraditionalVehicle);
            vehicleResult.IsSuccess.Should().BeTrue();
            var vehicle = vehicleResult.Value;

            var result = vehicle.ToString();

            result.Should().Be(expectedOutput);
        }

        public static Vehicle CreateVehicle()
        {
            var vin = "1A4GJ45R92J214567";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var plate = "ABC123";
            var plateStateProvince = State.CA;
            var unitNumber = "123456";
            var color = "Blue";
            var active = true;

            return Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active).Value;
        }

        public static class TestData
        {
            public static IEnumerable<object[]> InvalidMakes
            {
                get
                {
                    var nullString = new object[] { null };
                    var emptyString = new object[] { string.Empty };

                    var maxLengthResults = Enumerable.Range(Vehicle.MaximumMakeModelLength + 1, Vehicle.MaximumMakeModelLength + 10)
                        .Select(length => new string('A', length))
                        .Select(make => new object[] { make });

                    var minLengthResults = Enumerable.Range(0, Vehicle.MinimumMakeModelLength - 1)
                        .Select(length => new string('A', length))
                        .Where(make => make.Length < Vehicle.MinimumMakeModelLength)
                        .Select(make => new object[] { make });

                    return new[] { nullString, emptyString }.Concat(minLengthResults).Concat(maxLengthResults);
                }
            }

            public static IEnumerable<object[]> InvalidModels
            {
                get
                {
                    var nullString = new object[] { null };
                    var emptyString = new object[] { string.Empty };

                    var maxLengthResults = Enumerable.Range(Vehicle.MaximumMakeModelLength + 1, Vehicle.MaximumMakeModelLength + 10)
                        .Select(length => new string('A', length))
                        .Select(model => new object[] { model });

                    var minLengthResults = Enumerable.Range(0, Vehicle.MinimumMakeModelLength - 1)
                        .Select(length => new string('A', length))
                        .Where(model => model.Length < Vehicle.MinimumMakeModelLength)
                        .Select(model => new object[] { model });

                    return new[] { nullString, emptyString }.Concat(minLengthResults).Concat(maxLengthResults);
                }
            }

            public static IEnumerable<object[]> ValidYears =>
                new List<object[]>
                {
                    new object[] { Vehicle.YearMinimum },
                    new object[] { Vehicle.YearMinimum + 1 },
                    new object[] { DateTime.Today.Year },
                    new object[] { DateTime.Today.Year - 1 }
                };

            public static IEnumerable<object[]> InvalidYears =>
                new List<object[]>
                {
                    new object[] { Vehicle.YearMinimum - 1 },
                    new object[] { DateTime.Today.Year + 2 }
                };
        }

    }
}
