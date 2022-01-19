using System;
using System.ComponentModel.DataAnnotations;

namespace Janco.Idp.Areas.Identity.Data
{
    public class UserClaim : IConcurrencyAware
    {
        public UserClaim(string claimType, string claimValue)
        {
            if (!string.IsNullOrWhiteSpace(claimType)
             && !string.IsNullOrWhiteSpace(claimValue))
            {
                ClaimType = claimType;
                ClaimValue = claimValue;
            }
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string ClaimType { get; set; }

        [MaxLength(255)]
        [Required]
        public string ClaimValue { get; set; }

        [ConcurrencyCheck]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
