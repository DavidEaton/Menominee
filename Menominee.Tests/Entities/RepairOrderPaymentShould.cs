using Bogus;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderPaymentShould
    {
        private readonly Faker faker;

        public RepairOrderPaymentShould()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_RepairOrderPayment()
        {
            var paymentMethod = faker.PickRandom<PaymentMethod>();
            var amount = faker.Random.Double(0, 100);

            var result = RepairOrderPayment.Create(paymentMethod, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Amount.Should().Be(amount);
            result.Value.PaymentMethod.Should().Be(paymentMethod);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Undefined_PaymentMethod()
        {
            var paymentMethod = (PaymentMethod)(-1);
            var amount = faker.Random.Double(0, 100);

            var result = RepairOrderPayment.Create(paymentMethod, amount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPayment.RequiredMessage);
        }

        [Fact]
        public void Set_PaymentMethod_To_Valid_Value()
        {
            var paymentMethod = faker.PickRandom<PaymentMethod>();
            var amount = faker.Random.Double(0, 100);
            var repairOrderPayment = RepairOrderPayment.Create(paymentMethod, amount).Value;
            var newPaymentMethod = faker.PickRandom<PaymentMethod>();

            var result = repairOrderPayment.SetPaymentMethod(newPaymentMethod);

            result.IsSuccess.Should().BeTrue();
            repairOrderPayment.PaymentMethod.Should().Be(newPaymentMethod);
        }

        [Fact]
        public void Return_Failure_On_Set_Undefined_PaymentMethod()
        {
            var paymentMethod = faker.PickRandom<PaymentMethod>();
            var amount = faker.Random.Double(0, 100);
            var repairOrderPayment = RepairOrderPayment.Create(paymentMethod, amount).Value;
            var newPaymentMethod = (PaymentMethod)(-1);

            var result = repairOrderPayment.SetPaymentMethod(newPaymentMethod);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderPayment.RequiredMessage);
        }

        [Fact]
        public void Set_Amount_To_Valid_Value()
        {
            var paymentMethod = faker.PickRandom<PaymentMethod>();
            var amount = faker.Random.Double(0, 100);
            var repairOrderPayment = RepairOrderPayment.Create(paymentMethod, amount).Value;
            var newAmount = faker.Random.Double(0, 100);

            var result = repairOrderPayment.SetAmount(newAmount);

            result.IsSuccess.Should().BeTrue();
            repairOrderPayment.Amount.Should().Be(newAmount);
        }
    }
}
