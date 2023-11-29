using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Menominee.Api.Features.Users
{
    public class UserContext
    {
        private readonly IHttpContextAccessor _accessor;

        public UserContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public object Identity
        {
            get
            {
                return _accessor.HttpContext?.User.Identity;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                return _accessor.HttpContext?.User.Claims;
            }
        }
    }
}
