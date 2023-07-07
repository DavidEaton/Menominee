using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Enums;
using FluentAssertions;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RoleAssignmentShould
    {
        [Fact]
        public void Create_RoleAssignment()
        {
            var role = EmploymentRole.Technician;
            var result = RoleAssignment.Create(
                role);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<RoleAssignment>();
        }

        [Fact]
        public void SetRole()
        {
            var assignment = new RoleAssignmentFaker(true, EmploymentRole.Technician).Generate();
            assignment.Role.Should().Be(EmploymentRole.Technician);

            var result = assignment.SetRole(EmploymentRole.Counter);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(EmploymentRole.Counter);
            assignment.Role.Should().Be(EmploymentRole.Counter);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Role()
        {
            var assignment = new RoleAssignmentFaker(true, EmploymentRole.Technician).Generate();
            assignment.Role.Should().Be(EmploymentRole.Technician);

            var result = assignment.SetRole((EmploymentRole)(-1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RoleAssignment.RequiredMessage);
        }

        [Fact]
        public void Activate()
        {
            var assignment = new RoleAssignmentFaker(true).Generate();
            assignment.IsActive.Should().BeTrue();
            assignment.Deactivate();
            assignment.IsActive.Should().BeFalse();

            var result = assignment.Activate();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(true);
            assignment.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Deactivate()
        {
            var assignment = new RoleAssignmentFaker(true).Generate();
            assignment.IsActive.Should().BeTrue();
            var result = assignment.Deactivate();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(false);
            assignment.IsActive.Should().BeFalse();
        }

    }
}
