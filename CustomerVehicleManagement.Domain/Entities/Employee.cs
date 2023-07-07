using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Employee : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly DateTime StartDateMinimum = DateTime.Today.AddYears(-50);
        public static readonly DateTime EndDateMaximum = DateTime.Today.AddYears(1);
        public static readonly string DateRangeMessage = $"Employment date(s) invalid.";
        public static readonly int NoteMaximumLength = 10000;
        public static readonly string NoteMaximumLengthMessage = $"Notes cannot be over {NoteMaximumLength} characters in length.";
        public Person PersonalDetails { get; private set; }
        public DateTime? Hired { get; private set; }
        public DateTime? Exited { get; private set; }
        public string CompanyEmployeeId { get; set; }
        public IReadOnlyList<RoleAssignment> RoleAssignments => roleAssignments.ToList();
        private readonly List<RoleAssignment> roleAssignments = new();
        public string Notes { get; private set; }

        private Employee(Person personalDetails, List<RoleAssignment> roleAssignments, DateTime? hired = null, string notes = null)
        {
            PersonalDetails = personalDetails;
            Hired = hired;
            Notes = notes;

            if (roleAssignments is not null)
                foreach (var assignment in roleAssignments)
                    AddRoleAssignment(assignment);
        }

        public Result<RoleAssignment> AddRoleAssignment(RoleAssignment assignment)
        {
            if (assignment is null)
                return Result.Failure<RoleAssignment>(RequiredMessage);

            roleAssignments.Add(assignment);

            return Result.Success(assignment);
        }

        public static Result<Employee> Create(Person personalDetails, List<RoleAssignment> roleAssignments, DateTime? hired = null, string notes = null)
        {
            if (personalDetails is null)
                return Result.Failure<Employee>(RequiredMessage);

            if (hired.HasValue)
                if (hired < StartDateMinimum)
                    return Result.Failure<Employee>(DateRangeMessage);

            notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > NoteMaximumLength)
                return Result.Failure<Employee>(NoteMaximumLengthMessage);

            return Result.Success(new Employee(personalDetails, roleAssignments, hired, notes));
        }

        public Result<DateTime> SetHired(DateTime hired)
        {
            return hired < StartDateMinimum
                ? Result.Failure<DateTime>(DateRangeMessage)
                : Result.Success((DateTime)(Hired = hired));
        }

        public Result<DateTime> SetExited(DateTime exited)
        {
            return exited > EndDateMaximum
                ? Result.Failure<DateTime>(DateRangeMessage)
                : Result.Success((DateTime)(Hired = exited));
        }

        public Result<string> SetNotes(string notes)
        {
            return Result.Success(Notes = notes.Trim().Truncate(NoteMaximumLength));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Employee()
        {
            roleAssignments = new List<RoleAssignment>();
        }

        #endregion
    }
}
