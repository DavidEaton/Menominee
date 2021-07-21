using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Menominee.Idp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string Role { get; set; }
        public Guid TenantId { get; set; }

        [MaxLength(255)]
        public string TenantName { get; set; }
    }
}
