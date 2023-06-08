using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
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

            // Act
            var result = Vehicle.Create(vin, year, make, model);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var vehicle = result.Value;
            vehicle.VIN.Should().Be(vin);
            vehicle.Year.Should().Be(year);
            vehicle.Make.Should().Be(make);
            vehicle.Model.Should().Be(model);
        }

        [Theory]
        [InlineData("1A4GJ45R92J214567", 2010, "Mid Michigan", "Trailer")]
        [InlineData("1A4GJ45R92J214567", null, "Mid Michigan", "")]
        [InlineData("1A4GJ45R92J214567", null, "", "Trailer")]
        [InlineData("", null, "", "Trailer")]
        [InlineData(null, null, "", "Trailer")]
        [InlineData(null, null, null, "Trailer")]
        public void Create_NonTraditional_Vehicle(string vin, int? year, string make, string model)
        {
            var nonTraditionalVehicle = true;
            var result = Vehicle.Create(vin, year, make, model, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Vehicle>();
        }

        [Theory]
        [InlineData("", null, "", "")]
        [InlineData(null, null, null, null)]
        [InlineData("1A4GJ45R92J214567", null, "", "")]
        [InlineData("1A4GJ45R92J214567", 2010, "", "")]
        public void Not_Create_Invalid_NonTraditional_Vehicle(string vin, int? year, string make, string model)
        {
            var nonTraditionalVehicle = true;
            var result = Vehicle.Create(vin, year, make, model, nonTraditionalVehicle);

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

            var result = Vehicle.Create(vin, validYear, make, model);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(validYear);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1A4GJ45R92J21456")]
        [InlineData("5XYKUCA13BG0015")] // Only 16 characters long; too short
        [InlineData("5XYKUCA13BG0015178")] // Too long
        public void Not_Create_Vehicle_With_Invalid_Vin(string invalidVin)
        {
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";

            var result = Vehicle.Create(invalidVin, year, make, model);

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

            var result = Vehicle.Create(vin, year, make, invalidModel, nonTraditionalVehicle);

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

            var result = Vehicle.Create(vin, year, make, nonTraditionalModel, nonTraditionalVehicle);

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

            var result = Vehicle.Create(vin, year, nonTraditionalMake, model, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(year);
        }

        [Fact]
        public void Create_NonTraditionalVehicle_With_NonTraditional_Vin()
        {
            var nonTraditionalVin = "moops";
            var year = 2010;
            var make = "Honda";
            var model = "Pilot";
            var nonTraditionalVehicle = true;

            var result = Vehicle.Create(nonTraditionalVin, year, make, model, nonTraditionalVehicle);

            result.IsSuccess.Should().BeTrue();
            result.Value.Year.Should().Be(year);
        }

        [Fact]
        public void Create_NonTraditionalVehicle_With_Null_Year()
        {
            var vin = "1A4GJ45R92J214567";
            int? nonTraditionalYear = null;
            var make = "Honda";
            var model = "Pilot";
            var nonTraditionalVehicle = true;

            var result = Vehicle.Create(vin, nonTraditionalYear, make, model, nonTraditionalVehicle);

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

            var result = Vehicle.Create(vin, year, invalidMake, model, nonTraditionalVehicle);

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

            var result = Vehicle.Create(vin, invalidYear, make, model);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Vehicle.InvalidYearMessage);
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

        [Theory]
        [InlineData("1HGCM82633A123456", 2003, "Ho", "Ac", false, "2003 Ho Ac")]
        [InlineData("1HGCM82633A123456", null, "Ho", "Ac", false, "0 Ho Ac")]
        [InlineData("1HGCM82633A123456", 2003, "Honda", "Accord", false, "2003 Honda Accord")]
        [InlineData("1HGCM82633A123456", 2003, "Trailer", null, true, "2003 Trailer ")]
        [InlineData("1HGCM82633A123456", null, null, "Trailer", true, "0  Trailer")]
        [InlineData("1HGCM82633A123456", 2003, null, "Trailer", true, "2003  Trailer")]
        public void Format_ToString_Correctly(string vin, int? year, string make, string model, bool nonTraditionalVehicle, string expectedOutput)
        {
            var vehicleResult = Vehicle.Create(vin, year, make, model, nonTraditionalVehicle);
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

            return Vehicle.Create(vin, year, make, model).Value;
        }

        public static class TestData
        {
            public static IEnumerable<object[]> InvalidMakes
            {
                get
                {
                    var nullString = new object[] { null };
                    var emptyString = new object[] { string.Empty };

                    var maxLengthResults = Enumerable.Range(Vehicle.MaximumLength + 1, Vehicle.MaximumLength + 10)
                        .Select(length => new string('A', length))
                        .Select(make => new object[] { make });

                    var minLengthResults = Enumerable.Range(0, Vehicle.MinimumLength - 1)
                        .Select(length => new string('A', length))
                        .Where(make => make.Length < Vehicle.MinimumLength)
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

                    var maxLengthResults = Enumerable.Range(Vehicle.MaximumLength + 1, Vehicle.MaximumLength + 10)
                        .Select(length => new string('A', length))
                        .Select(model => new object[] { model });

                    var minLengthResults = Enumerable.Range(0, Vehicle.MinimumLength - 1)
                        .Select(length => new string('A', length))
                        .Where(model => model.Length < Vehicle.MinimumLength)
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
