using Microsoft.AspNetCore.Identity;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menominee.Idp.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string Role { get; set; }
        public Guid TenantId { get; set; }

        [MaxLength(255)]
        public string TenantName { get; set; }

        public ShopRole ShopRole { get; set; }
        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
