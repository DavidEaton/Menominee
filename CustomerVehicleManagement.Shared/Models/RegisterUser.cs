using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Shared.Models
{
    public class RegisterUser
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // TODO: Move validation values to centralized setting or service; improve encapsulation

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Guid TenantId { get; set; }

        public string TenantName { get; set; }

        [Required]
        public string ShopRole { get; set; }
    }
}
