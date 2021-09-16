using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policies.CanManageUsers)]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly UserContext UserContext;
        public UsersController(IConfiguration configuration,
                               UserContext userContext)
        {
            Configuration = configuration;
            UserContext = userContext;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<UserListDto>> GetUsers()
        {
            var tenantId = GetTenantId();

            var users = new List<UserListDto>();

            using SqlConnection connection = new(Configuration[$"IDPSettings:Connection"]);
            try
            {
                connection.Open();
                string query = $"SELECT * FROM [dbo].[AspNetUsers] WHERE [TenantId] = '{tenantId}';";
                using SqlCommand command = new(query, connection);
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(
                        new UserListDto()
                        {
                            Id = (string)reader["Id"],
                            Username = reader["Username"].ToString(),
                            Name = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            ShopRole = Enum.Parse<ShopRole>((string)reader["ShopRole"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError($"Exception message from GetUsersAsync(): {ex.Message}");
                return null;
            }

            return Ok(users);
        }

        public Guid GetTenantId()
        {
            if (UserContext == null)
                return new Guid();

            var claims = UserContext.Claims;
            Guid tenantId;

            try
            {
                tenantId = Guid.Parse(claims.First(claim => claim.Type == "tenantId").Value);
            }
            catch (Exception ex)
            {
                //Logger.LogError($"Exception message from GetTenantId(): {ex.Message}");
                return new Guid();
            }

            return tenantId;
        }
    }
}
