using System;
using System.Linq;
using Menominee.Api.Users;
using Menominee.Common.Enums;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Menominee.Api.Common
{
    public abstract class BaseControllerApplicationUser<T> : BaseApplicationController<T>
    {
        protected readonly UserContext UserContext;
        protected readonly IMSGraphUserService GraphUserService;
        
        public BaseControllerApplicationUser(UserContext userContext,
            IMSGraphUserService graphUserService, ILogger<T> logger) : base(logger)
        {
            UserContext = userContext;
            GraphUserService = graphUserService;
        }

        protected ShopRole? GetShopRole()
        {
            Logger.LogTrace("GetShopRole");
            ShopRole? role = null;

            try
            {
                if (UserContext != null)
                {
                    var claims = UserContext.Claims;
                    var stringRole = claims.First(claim => claim.Type == GraphUserService.ShopRoleAttributeName).Value;
                    role = Enum.TryParse(stringRole, true, out ShopRole val) ? val : null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetShopRole-Catch");
                role = null;
            }

            Logger.LogTrace("GetShopRole - DONE");
            return role;
        }
        
        protected Guid GetShopId()
        {
            Logger.LogTrace("GetShopId");
            // TODO: evaluate if we really want to create a GUID or should we use empty
            var id = new Guid();

            try
            {
                if (UserContext != null)
                {
                    var claims = UserContext.Claims;
                    id = Guid.Parse(claims.First(claim => claim.Type == GraphUserService.ShopIdAttributeName).Value);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetShopId-Catch");
                // TODO: evaluate if we really want to create a GUID or should we use empty
                id = new Guid();
            }
            Logger.LogTrace("GetShopId - DONE");
            return id;
        }

        protected string GetShopName()
        {
            Logger.LogTrace("GetShopName");
            var shopName = string.Empty;
            
            try
            {
                if (UserContext != null)
                {
                    var claims = UserContext.Claims;
                    shopName = claims.First(claim => claim.Type == GraphUserService.ShopNameAttributeName).Value;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetShopName-Catch");
                shopName = string.Empty;
            }
            Logger.LogTrace("GetShopName");
            return shopName;
        }
    }
}