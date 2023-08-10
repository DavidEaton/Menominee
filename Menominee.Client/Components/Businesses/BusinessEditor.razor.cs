using Menominee.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Businesses
{
    public partial class BusinessEditor : ComponentBase
    {
        [Parameter]
        public BusinessToWrite Business { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private BusinessToWrite businessOriginal;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                businessOriginal = new BusinessToWrite
                {
                    Name = Business.Name,
                    Notes = Business.Notes,
                    Address = Business.Address,
                    Emails = Business.Emails,
                    Phones = Business.Phones,
                    Contact = Business.Contact
                };
            }
        }

        public void Reset()
        {
            Business.Name = businessOriginal.Name;
            Business.Notes = businessOriginal.Notes;
            Business.Address = businessOriginal.Address;
            Business.Emails = businessOriginal.Emails;
            Business.Phones = businessOriginal.Phones;
            Business.Contact = businessOriginal.Contact;
        }
    }
}
