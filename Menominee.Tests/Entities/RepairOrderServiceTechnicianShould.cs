using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderServiceTechnicianShould
    {
        [Fact]
        public void Create_RepairOrderServiceTechnician()
        {
            var employee = new EmployeeFaker(true).Generate();

            var result = RepairOrderServiceTechnician.Create(employee);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<RepairOrderServiceTechnician>();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderServiceTechnician_With_Invalid_Employee()
        {
            var result = RepairOrderServiceTechnician.Create(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderServiceTechnician.RequiredMessage);
        }

        [Fact]
        public void Return_Success_On_Create_RepairOrderServiceTechnician_With_Different_Employee()
        {
            var employee1 = new EmployeeFaker(true).Generate(); // Employee 1
            var employee2 = new EmployeeFaker(true).Generate(); // Employee 2

            var result1 = RepairOrderServiceTechnician.Create(employee1);
            var result2 = RepairOrderServiceTechnician.Create(employee2);

            result1.IsSuccess.Should().BeTrue();
            result1.Value.Employee.Should().Be(employee1);

            result2.IsSuccess.Should().BeTrue();
            result2.Value.Employee.Should().Be(employee2);

            result1.Value.Employee.Should().NotBe(result2.Value.Employee);
        }

    }
}
