using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace CustomerVehicleManagement.Api.Users
{
    public class UserContext
    {
        private readonly IHttpContextAccessor accessor;

        public UserContext(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public object Identity
        {
            get
            {
                return accessor.HttpContext.User.Identity;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                return accessor.HttpContext.User.Claims;
            }
        }
    }
}
