using FluentAssertions;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Fakers
{
    public class SalesTaxFakerTests
    {
        [Theory]
        [InlineData(false, 0, true, 0)]
        [InlineData(false, 1, true, 1)]
        [InlineData(true, 0, false, 0)]
        [InlineData(true, 1, true, 1)]
        public void SalesTaxFaker_Id_Behaves_As_Expected_With_Various_Inputs(bool generateId, long id, bool checkForSpecificId, long specificId)
        {
            var faker = new SalesTaxFaker(generateId, id);

            var salesTax = faker.Generate();

            if (checkForSpecificId)
                salesTax.Id.Should().Be(specificId);
            else
                salesTax.Id.Should().NotBe(0);
        }

        [Fact]
        public void SalesTaxFaker_Id_Is_Zero_When_GenerateId_Is_False_And_Id_Is_Zero()
        {
            var faker = new SalesTaxFaker(false, 0);

            var salesTax = faker.Generate();

            salesTax.Id.Should().Be(0);
        }

        [Fact]
        public void SalesTaxFaker_Id_Is_One_When_GenerateId_Is_False_And_Id_Is_One()
        {
            var faker = new SalesTaxFaker(false, 1);

            var salesTax = faker.Generate();

            salesTax.Id.Should().Be(1);
        }

        [Fact]
        public void SalesTaxFaker_Id_Is_Not_Zero_When_GenerateId_Is_True_And_Id_Is_Zero()
        {
            var faker = new SalesTaxFaker(true, 0);

            var salesTax = faker.Generate();

            salesTax.Id.Should().NotBe(0);
        }

        [Fact]
        public void SalesTaxFaker_Id_Is_One_When_GenerateId_Is_True_And_Id_Is_One()
        {
            var faker = new SalesTaxFaker(true, 1);

            var salesTax = faker.Generate();

            salesTax.Id.Should().Be(1);
        }
    }
}
