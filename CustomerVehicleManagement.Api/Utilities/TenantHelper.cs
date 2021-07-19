using CustomerVehicleManagement.Api.User;
using Serilog;
using System;
using System.Linq;

namespace CustomerVehicleManagement.Api.Utilities
{
    public class TenantHelper
    {
        public static string GetTenantId(UserContext UserContext)
        {
            var claims = UserContext.Claims;
            var tenantId = string.Empty;
            try
            {
                tenantId = claims.First(c => c.Type == "tenantId").Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from TenantHelper.GetTenantId(): {ex.Message}");
                return string.Empty;
            }

            return tenantId;
        }

        public static string GetTenantName(UserContext UserContext)
        {
            var claims = UserContext.Claims;
            var tenantName = string.Empty;
            try
            {
                tenantName = claims.First(c => c.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from TenantHelper.GetTenantName(): {ex.Message}");
                return string.Empty;
            }

            return tenantName;
        }
    }
}

