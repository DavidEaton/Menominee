using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderItemTaxShould
    {
        [Fact]
        public void Create_RepairOrderItemTax()
        {
            var partTax = PartTax.Create(0.05, 100).Value;
            var laborTax = LaborTax.Create(0.10, 200).Value;

            var result = RepairOrderItemTax.Create(partTax, laborTax);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<RepairOrderItemTax>();
            result.Value.PartTax.Should().Be(partTax);
            result.Value.LaborTax.Should().Be(laborTax);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderItemTax_With_Null_PartTax()
        {
            var laborTax = LaborTax.Create(0.10, 200).Value;

            var result = RepairOrderItemTax.Create(null, laborTax);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemTax.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderItemTax_With_Null_LaborTax()
        {
            var partTax = PartTax.Create(0.05, 100).Value;

            var result = RepairOrderItemTax.Create(partTax, null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemTax.RequiredMessage);
        }

        [Fact]
        public void SetPartTax()
        {
            var partTax = PartTax.Create(0.05, 16).Value;
            var laborTax = LaborTax.Create(0.3, 15).Value;
            var repairOrderItemTax = RepairOrderItemTax.Create(partTax, laborTax).Value;
            repairOrderItemTax.PartTax.Should().Be(partTax);
            var updatedPartTax = PartTax.Create(0.09, 26).Value;

            var result = repairOrderItemTax.SetPartTax(updatedPartTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderItemTax.PartTax.Should().Be(updatedPartTax);
        }

        [Fact]
        public void Return_Failure_On_SetPartTax_With_Null_PartTax()
        {
            var partTax = PartTax.Create(0.05, 16).Value;
            var laborTax = LaborTax.Create(0.3, 15).Value;
            var repairOrderItemTax = RepairOrderItemTax.Create(partTax, laborTax).Value;

            var result = repairOrderItemTax.SetPartTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemTax.RequiredMessage);
        }

        [Fact]
        public void SetLaborTax()
        {
            var partTax = PartTax.Create(0.05, 16).Value;
            var laborTax = LaborTax.Create(0.3, 15).Value;
            var repairOrderItemTax = RepairOrderItemTax.Create(partTax, laborTax).Value;
            repairOrderItemTax.LaborTax.Should().Be(laborTax);
            var updatedlaborTax = LaborTax.Create(0.09, 26).Value;

            var result = repairOrderItemTax.SetLaborTax(updatedlaborTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderItemTax.LaborTax.Should().Be(updatedlaborTax);
        }

        [Fact]
        public void Return_Failure_On_SetLaborTax_With_Null_LaborTax()
        {
            var partTax = PartTax.Create(0.05, 16).Value;
            var laborTax = LaborTax.Create(0.3, 15).Value;
            var repairOrderItemTax = RepairOrderItemTax.Create(partTax, laborTax).Value;

            var result = repairOrderItemTax.SetLaborTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemTax.RequiredMessage);
        }

    }
}
