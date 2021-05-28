﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Menominee.Idp.Areas.Identity.Data
{
    public class AspNetTenant
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public string CompanyName { get; set; }

        [MaxLength(1020)]
        public string LogoUrl { get; set; }
    }
}