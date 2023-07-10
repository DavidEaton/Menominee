using Azure;
using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Extensions;
using System;
using Telerik.SvgIcons;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class EmployeeShould
    {
        private readonly Faker faker = new();

        [Fact]
        public void Create_Employee()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Return_Failure_On_Create_EmployeeWith_Null_Person()
        {
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var result = Employee.Create(
                null,
                roleAssignments,
                hired,
                notes);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_Employee_With_Hired_Date_Earlier_Than_StartDateMinimum()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = Employee.StartDateMinimum.AddDays(-1); // Invalid hired date
            var notes = faker.Random.String2(1, 500);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void Create_Employee_With_Notes_Length_Greater_Than_NoteMaximumLength_Truncated()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(Employee.NoteMaximumLength + 1); // Invalid notes length

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Create_Employee_With_Null_RoleAssignments()
        {
            var person = new PersonFaker(true).Generate();
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);

            var result = Employee.Create(
                person,
                null,
                hired,
                notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Return_Failure_On_SetHired_With_Date_Earlier_Than_StartDateMinimum()
        {
            var employee = new EmployeeFaker(true).Generate();
            var hired = Employee.StartDateMinimum.AddDays(-1); // Invalid hired date

            var result = employee.SetHired(hired);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void SetHired()
        {
            var employee = new EmployeeFaker(true).Generate();
            var hired = DateTime.Today;

            var result = employee.SetHired(hired);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(hired);
        }

        [Fact]
        public void Return_Failure_On_SetExited_With_Date_Later_Than_EndDateMaximum()
        {
            var employee = new EmployeeFaker(true).Generate();
            var exited = Employee.EndDateMaximum.AddDays(1); // Invalid exited date

            var result = employee.SetExited(exited);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void SetExited()
        {
            var employee = new EmployeeFaker(true).Generate();
            var exited = DateTime.Today;

            var result = employee.SetExited(exited);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(exited);
        }

        [Fact]
        public void SetNotes_With_Length_Greater_Than_NoteMaximumLength_Truncated()
        {
            var employee = new EmployeeFaker(true).Generate();
            var notes = faker.Random.String2(Employee.NoteMaximumLength + 1); // Invalid notes length

            var result = employee.SetNotes(notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(
                notes.Trim()
                .Truncate(Employee.NoteMaximumLength));
        }

        [Fact]
        public void SetNotes()
        {
            var employee = new EmployeeFaker(true).Generate();
        
            var notes = faker.Random.String2(1, Employee.NoteMaximumLength); // Valid notes length

            var result = employee.SetNotes(notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(notes);
        }
        [Fact]
        public void Return_Failure_On_AddRoleAssignment_With_Null_Assignment()
        {
            var employee = new EmployeeFaker(true).Generate();
            RoleAssignment assignment = null; // Null assignment

            var result = employee.AddRoleAssignment(assignment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.RequiredMessage);
        }

        [Fact]
        public void AddRoleAssignment()
        {
            var employee = new EmployeeFaker(true).Generate();
            var assignment = new RoleAssignmentFaker(true).Generate(); // Valid assignment

            var result = employee.AddRoleAssignment(assignment);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(assignment);
        }
    }
}
