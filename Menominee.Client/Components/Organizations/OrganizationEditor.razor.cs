using Menominee.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Organizations
{
    public partial class OrganizationEditor : ComponentBase
    {
        [Parameter]
        public OrganizationToWrite Organization { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private OrganizationToWrite organizationOriginal;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                organizationOriginal = new OrganizationToWrite
                {
                    Name = Organization.Name,
                    Notes = Organization.Notes,
                    Address = Organization.Address,
                    Emails = Organization.Emails,
                    Phones = Organization.Phones,
                    Contact = Organization.Contact
                };
            }
        }

        public void Reset()
        {
            Organization.Name = organizationOriginal.Name;
            Organization.Notes = organizationOriginal.Notes;
            Organization.Address = organizationOriginal.Address;
            Organization.Emails = organizationOriginal.Emails;
            Organization.Phones = organizationOriginal.Phones;
            Organization.Contact = organizationOriginal.Contact;
        }
    }
}
