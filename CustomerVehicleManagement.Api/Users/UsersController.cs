using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Data;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Users
{
    //[Authorize(Policies.CanManageUsers)]
    public class UsersController : ApplicationController
    {
        private readonly IdentityUserDbContext dbContext;
        private readonly UserContext userContext;

        public UsersController(IdentityUserDbContext dbContext, UserContext userContext)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
        }

        public ActionResult<IReadOnlyList<UserToRead>> GetUsers()
        {
            var tenantId = GetTenantId();
            var users = dbContext.ApplicationUsers.Where(x => x.TenantId == tenantId).Select(x => new UserToRead()
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.UserName,
                Username = x.UserName
            }).ToList();

            return Ok(users);
        }

        public Guid GetTenantId()
        {
            if (userContext == null)
                return new Guid();

            var claims = userContext.Claims;

            try
            {
                return Guid.Parse(claims.First(claim => claim.Type == "tenantId").Value);
            }
            catch (Exception ex)
            {
                //Logger.LogError($"Exception message from GetTenantId(): {ex.Message}");
                return new Guid();
            }
        }
    }
}

