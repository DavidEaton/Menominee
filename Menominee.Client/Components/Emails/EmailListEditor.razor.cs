using Menominee.Shared.Models.Contactable;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.Blazor;

namespace Menominee.Client.Components.Emails
{
    public partial class EmailListEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite>? Emails { get; set; } = null;

        [Parameter]
        public FormMode FormMode { get; set; }

        [CascadingParameter]
        public DialogFactory? Dialogs { get; set; }

        private IEnumerable<EmailToWrite> SelectedEmails { get; set; } = Enumerable.Empty<EmailToWrite>();
        private EmailToWrite? SelectedEmail { get; set; }
        private EmailToWrite? EmailToModify { get; set; } = null;

        private TelerikGrid<EmailToWrite>? Grid { get; set; }

        private int SelectedEmailIndex
        {
            get => selectedEmailIndex;
            set
            {
                selectedEmailIndex = value;
                CanEditEmail = selectedEmailIndex >= 0;
                CanDeleteEmail = CanEdit && selectedEmailIndex >= 0;
            }
        }

        private bool CanEdit { get; set; } = false;

        private long emailIdToSelect = 0;
        private int selectedEmailIndex = -1;
        private bool editDialogVisible = false;
        private bool parametersSet = false;

        private FormMode EmailFormMode { get; set; } = FormMode.Unknown;
        private bool CanEditEmail { get; set; } = false;
        private bool CanDeleteEmail { get; set; } = false;

        private bool EditDialogVisible
        {
            get => editDialogVisible;
            set
            {
                if (value)
                {
                    if (EmailFormMode == FormMode.Add)
                    {
                        EmailToModify = new();
                        if (Emails?.Count == 0)
                        {
                            EmailToModify.IsPrimary = true;
                        }
                    }

                    if ((EmailFormMode is FormMode.Edit or FormMode.View) && 
                        SelectedEmail is not null)
                    {
                        EmailToModify = new();
                        CopyEmail(SelectedEmail, EmailToModify);
                    }
                }
                else
                {
                    if (EmailToModify is not null)
                    {
                        EmailToModify = null;
                    }

                    EmailFormMode = FormMode.Unknown;
                }

                editDialogVisible = value;
            }
        }

        protected override void OnParametersSet()
        {
            if (parametersSet)
            {
                return;
            }
            parametersSet = true;

            CanEdit = FormMode is FormMode.Add or FormMode.Edit;

            if (Emails?.Count > 0)
            {
                if (emailIdToSelect == 0)
                {
                    SelectedEmail = Emails.FirstOrDefault();
                }

                if (SelectedEmail is not null)
                {
                    SelectedEmailIndex = Emails.IndexOf(SelectedEmail);
                    SelectedEmails = new List<EmailToWrite> { SelectedEmail };
                }
            }
        }

        private void OnEdit()
        {
            if (CanEditEmail)
            {
                EmailFormMode = CanEdit ? FormMode.Edit : FormMode.View;
                EditDialogVisible = true;
            }
        }

        private void OnNew()
        {
            EmailFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private async Task OnDelete()
        {
            if (SelectedEmail is not null && 
                Emails is not null && 
                Dialogs is not null &&
                await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedEmail.Address}?", "Remove Email"))
            {
                Emails.Remove(SelectedEmail);
                SelectedEmail = Emails.FirstOrDefault();
                if (SelectedEmail is not null)
                {
                    SelectedEmails = new List<EmailToWrite> { SelectedEmail };
                    SelectedEmailIndex = Emails.IndexOf(SelectedEmail);
                }
                else
                {
                    SelectedEmailIndex = -1;
                }
                Grid?.Rebind();
            }
        }

        private void OnSaveEdit()
        {
            if (EmailFormMode is not FormMode.Add and not FormMode.Edit)
            {
                return;
            }
            if (Emails is null || EmailToModify is null)
            {
                return;
            }

            if (EmailFormMode == FormMode.Add)
            {
                Emails.Add(EmailToModify);
                SelectedEmailIndex = Emails.IndexOf(EmailToModify);
                SelectedEmail = Emails[SelectedEmailIndex];
                SelectedEmails = new List<EmailToWrite> { SelectedEmail };
                Grid?.Rebind();
            }
            else if (EmailFormMode == FormMode.Edit)
            {
                CopyEmail(EmailToModify, Emails[SelectedEmailIndex]);
            }
            EditDialogVisible = false;
            StateHasChanged();
        }

        private void OnCancelEdit()
        {
            EmailFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedEmail = args.Item as EmailToWrite;
            if (SelectedEmail is not null)
            {
                SelectedEmailIndex = Emails?.IndexOf(SelectedEmail) ?? -1;
                SelectedEmails = new List<EmailToWrite> { SelectedEmail };
            }
        }

        private static void CopyEmail(EmailToWrite src, EmailToWrite dst)
        {
            dst.Address = src.Address;
            dst.IsPrimary = src.IsPrimary;
        }
    }
}
