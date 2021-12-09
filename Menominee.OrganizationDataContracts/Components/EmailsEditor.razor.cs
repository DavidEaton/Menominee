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
        public IList<EmailToWrite> EmailsToWrite { get; set; }
        public EmailToWrite EmailToWrite { get; set; }

        private bool DialogVisible => EmailToWrite != null && (Adding || Editing);
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private TelerikTextBox EmailAddressControl { get; set; }

        private async Task AddAsync()
        {
            EmailToWrite = new();

            if (EmailAddressControl != null)
                await EmailAddressControl.FocusAsync();

            Adding = true;
        }

        private void Edit(EmailToWrite item)
        {
            EmailToWrite = item;
            Editing = true;
        }

        private void Save()
        {
            if (Adding)
                EmailsToWrite.Add(EmailToWrite);

            Adding = false;
            Editing = false;
        }

        private void Cancel()
        {
            if (EmailToWrite != null && Adding)
                EmailToWrite = null;

            Adding = false;
            Editing = false;
        }
    }
}
