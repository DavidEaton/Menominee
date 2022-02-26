using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

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
        public FormMode FormMode { get; set; }

        private EmailToWrite emailOriginal;

        private bool DialogVisible => Email != null && (Adding || Editing);
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;

        public void Reset()
        {
            Email.Address = emailOriginal.Address;
            Email.IsPrimary = emailOriginal.IsPrimary;
        }

        private void Add()
        {
            Email = new();
            Adding = true;
        }

        private void Edit(EmailToWrite item)
        {
            if (item is not null)
            {
                Email = item;
                Editing = true;

                emailOriginal = new EmailToWrite
                {
                    Address = Email.Address,
                    IsPrimary = Email.IsPrimary
                };
            }
        }

        private void Save()
        {
            if (Email != null && Adding)
            {
                Emails.Add(Email);
                Adding = false;
            }

            if (Email != null && Editing)
                Editing = false;
        }

        private void CancelEdit()
        {
            if (Email != null && Adding)
                Email = new();

            if (Email != null && Editing)
                Reset();

            Adding = false;
            Editing = false;
        }
    }
}
