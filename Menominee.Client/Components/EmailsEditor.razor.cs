using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite> EmailsToWrite { get; set; }
        public EmailToWrite EmailToWrite { get; set; }

        private bool DialogVisible => Adding || Editing;
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private TelerikTextBox EmailAddressControl { get; set; }

        private async Task AddAsync()
        {
            EmailToWrite = new();
            Adding = true;

            if (EmailAddressControl != null)
                await EmailAddressControl.FocusAsync();
        }

        private async Task EditAsync(EmailToWrite item)
        {
            EmailToWrite = item;
            Editing = true;

            if (EmailAddressControl != null)
                await EmailAddressControl.FocusAsync();
        }

        private void Save()
        {
            if (EmailToWrite != null && Adding)
            {
                EmailsToWrite.Add(EmailToWrite);
                Adding = false;
            }

            if (EmailToWrite != null && Editing)
                Editing = false;
        }

        private void CancelEdit()
        {
            Adding = false;
            Editing = false;
        }
    }
}
