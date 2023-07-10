using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderServiceTaxShould
    {
        private readonly PartTax partTax = null;
        private readonly LaborTax laborTax = null;

        public RepairOrderServiceTaxShould()
        {
            partTax = PartTax.Create(.06, 5.78).Value;
            laborTax = LaborTax.Create(.04, 3.90).Value;
        }

        [Fact]
        public void Create_RepairOrderServiceTax()
        {
            var result = RepairOrderServiceTax.Create(partTax, laborTax);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<RepairOrderServiceTax>();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderServiceTax_With_Null_PartTax()
        {
            var result = RepairOrderServiceTax.Create(null, laborTax);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTax.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderServiceTax_With_Null_LaborTax()
        {
            var result = RepairOrderServiceTax.Create(partTax, null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTax.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderServiceTax_With_Null_LaborTax_PartTax()
        {
            var result = RepairOrderServiceTax.Create(null, null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTax.RequiredMessage);
        }

        [Fact]
        public void Set_PartTax_Successfully()
        {
            var repairOrderServiceTax = RepairOrderServiceTax.Create(partTax, laborTax).Value;

            var newPartTax = PartTax.Create(1.06, 15.78).Value;
            var result = repairOrderServiceTax.SetPartTax(newPartTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderServiceTax.PartTax.Should().Be(newPartTax);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_PartTax()
        {
            var repairOrderServiceTax = RepairOrderServiceTax.Create(partTax, laborTax).Value;

            var result = repairOrderServiceTax.SetPartTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTax.RequiredMessage);
        }

        [Fact]
        public void Set_LaborTax_Successfully()
        {
            var repairOrderServiceTax = RepairOrderServiceTax.Create(partTax, laborTax).Value;

            var newLaborTax = LaborTax.Create(1.04, 13.90).Value;
            var result = repairOrderServiceTax.SetLaborTax(newLaborTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderServiceTax.LaborTax.Should().Be(newLaborTax);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_LaborTax()
        {
            var repairOrderServiceTax = RepairOrderServiceTax.Create(partTax, laborTax).Value;

            var result = repairOrderServiceTax.SetLaborTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTax.RequiredMessage);
        }
    }
}
