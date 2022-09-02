using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class SaleCodeShould
    {
        [Fact]
        public void Create_SaleCode()
        {
            // Arrange
            string name = "Sale Code One";
            string code = "SC1";
            double laborRate = .25;
            double desiredMargin = .25;
            SaleCodeShopSupplies shopSupplies = new();

            // Act
            var saleCode = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value;

            // Assert
            saleCode.Name.Should().Be(name);
            saleCode.Code.Should().Be(code);
            saleCode.LaborRate.Should().Be(laborRate);
            saleCode.DesiredMargin.Should().Be(desiredMargin);
            saleCode.ShopSupplies.Should().Be(shopSupplies);
        }
    }
}
