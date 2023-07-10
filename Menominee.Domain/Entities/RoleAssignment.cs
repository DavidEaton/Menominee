using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities
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
            return
                !Enum.IsDefined(typeof(EmploymentRole), role)
                ? Result.Failure<RoleAssignment>(RequiredMessage)
                : Result.Success(new RoleAssignment(role));
        }

        public Result<EmploymentRole> SetRole(EmploymentRole role)
        {
            return
                !Enum.IsDefined(typeof(EmploymentRole), role)
                ? Result.Failure<EmploymentRole>(RequiredMessage)
                : Result.Success(Role = role);
        }

        public Result<bool> Activate()
        {
            return Result.Success(IsActive = true);
        }

        public Result<bool> Deactivate()
        {
            return Result.Success(IsActive = false);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RoleAssignment() { }

        #endregion

    }
}
