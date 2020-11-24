using System;

namespace Client.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }
    }
}
