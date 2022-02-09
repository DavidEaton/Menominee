using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite> Emails { get; set; }
        public EmailToWrite Email { get; set; }

        private bool DialogVisible => Email != null && (Adding || Editing);
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private TelerikTextBox EmailAddressControl { get; set; }

        private async Task AddAsync()
        {
            Email = new();

            if (EmailAddressControl != null)
                await EmailAddressControl.FocusAsync();

            Adding = true;
        }

        private void Edit(EmailToWrite item)
        {
            Email = item;
            Editing = true;
        }

        private void Save()
        {
            if (Adding)
                Emails.Add(Email);

            Adding = false;
            Editing = false;
        }

        private void Cancel()
        {
            if (Email != null && Adding)
                Email = null;

            Adding = false;
            Editing = false;
        }
    }
}
