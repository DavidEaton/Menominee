using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor;

namespace Menominee.Client.Components.Emails
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite> Emails { get; set; }
        public EmailToWrite Email { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; } = FormMode.Unknown;

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private EmailToWrite emailOriginal;

        public void Reset()
        {
            Email.Address = emailOriginal.Address;
            Email.IsPrimary = emailOriginal.IsPrimary;
        }

        private void Add()
        {
            Email = new();
            FormMode = FormMode.Add;
        }

        private void Edit(EmailToWrite email)
        {
            if (email is not null)
            {
                Email = email;
                FormMode = FormMode.Edit;

                emailOriginal = new EmailToWrite
                {
                    Address = Email.Address,
                    IsPrimary = Email.IsPrimary
                };
            }
        }

        private void Save()
        {
            if (Email != null && FormMode == FormMode.Add)
                Emails.Add(Email);

            FormMode = FormMode.Unknown;
        }

        private void Cancel()
        {
            if (Email != null && FormMode == FormMode.Add)
                Email = new();

            if (Email != null && FormMode == FormMode.Edit)
                Reset();

            FormMode = FormMode.Unknown;
        }

        private async Task RemoveAsync()
        {
            if (await RemoveConfirm())
            {
                Emails.Remove(Email);
                FormMode = FormMode.Unknown;
            }
        }

        public async Task<bool> RemoveConfirm()
        {
            return await Dialogs.ConfirmAsync($"Are you sure you want to remove email address {Email.Address}?", "Remove Email");
        }
    }
}
