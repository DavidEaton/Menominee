using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class RoleAssignment : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public EmploymentRole Role { get; private set; }
        public bool IsActive { get; private set; }

        private RoleAssignment(EmploymentRole role)
        {
            Role = role;
            IsActive = true;
        }

        public static Result<RoleAssignment> Create(EmploymentRole role)
        {
            if (!Enum.IsDefined(typeof(EmploymentRole), role))
                return Result.Failure<RoleAssignment>(RequiredMessage);

            return Result.Success(new RoleAssignment(role));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RoleAssignment() { }

        #endregion

    }
}
