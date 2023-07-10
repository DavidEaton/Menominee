using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;
using Telerik.Blazor;
using Menominee.Shared.Models.Contactable;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Phones
{
    public partial class PhoneListEditor : ComponentBase
    {
        [Parameter]
        public IList<PhoneToWrite> Phones { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public PhoneType DefaultPhoneType { get; set; } = PhoneType.Mobile;

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private IEnumerable<PhoneToWrite> SelectedPhones { get; set; } = Enumerable.Empty<PhoneToWrite>();
        private PhoneToWrite SelectedPhone { get; set; }
        private PhoneToWrite PhoneToModify { get; set; } = null;

        private TelerikGrid<PhoneToWrite> Grid { get; set; }

        private int SelectedPhoneIndex
        {
            get => selectedPhoneIndex;
            set
            {
                selectedPhoneIndex = value;
                CanEditPhone = selectedPhoneIndex >= 0;
                CanDeletePhone = CanEdit && selectedPhoneIndex >= 0;
            }
        }

        private bool CanEdit { get; set; } = false;

        private long phoneIdToSelect { get; set; } = 0;
        private int selectedPhoneIndex = -1;
        private bool editDialogVisible = false;

        private FormMode PhoneFormMode { get; set; } = FormMode.Unknown;
        private bool CanEditPhone { get; set; } = false;
        private bool CanDeletePhone { get; set; } = false;

        private bool EditDialogVisible
        {
            get => editDialogVisible;
            set
            {
                if (value == true)
                {
                    if (PhoneFormMode == FormMode.Add)
                    {
                        PhoneToModify = new PhoneToWrite();
                        PhoneToModify.PhoneType = DefaultPhoneType;
                        if (Phones.Count == 0)
                            PhoneToModify.IsPrimary = true;
                    }

                    if (PhoneFormMode == FormMode.Edit || PhoneFormMode == FormMode.View)
                    {
                        PhoneToModify = new PhoneToWrite();
                        CopyPhone(SelectedPhone, PhoneToModify);
                    }
                }
                else
                {
                    if (PhoneToModify is not null)
                        PhoneToModify = null;
                    PhoneFormMode = FormMode.Unknown;
                }

                editDialogVisible = value;
            }
        }

        protected override void OnParametersSet()
        {
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;

            if (Phones?.Count > 0)
            {
                if (phoneIdToSelect == 0)
                    SelectedPhone = Phones.FirstOrDefault();

                SelectedPhoneIndex = Phones.IndexOf(SelectedPhone);
                SelectedPhones = new List<PhoneToWrite> { SelectedPhone };
            }
        }

        private void OnEdit()
        {
            if (CanEditPhone)
            {
                PhoneFormMode = CanEdit ? FormMode.Edit : FormMode.View;
                EditDialogVisible = true;
            }
        }

        private void OnNew()
        {
            PhoneFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private async Task OnDelete()
        {
            if (SelectedPhone is not null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedPhone.Number.ToString()}?", "Remove Phone"))
            {
                Phones.Remove(SelectedPhone);
                SelectedPhone = Phones.FirstOrDefault();
                SelectedPhones = new List<PhoneToWrite> { SelectedPhone };
                SelectedPhoneIndex = Phones.IndexOf(SelectedPhone);
                Grid.Rebind();
            }
        }

        private void OnSaveEdit()
        {
            if (PhoneFormMode != FormMode.Add && PhoneFormMode != FormMode.Edit)
                return;

            if (PhoneFormMode == FormMode.Add)
            {
                Phones.Add(PhoneToModify);
                SelectedPhoneIndex = Phones.IndexOf(PhoneToModify);
                SelectedPhone = Phones[SelectedPhoneIndex];
                SelectedPhones = new List<PhoneToWrite> { SelectedPhone };
                Grid.Rebind();
            }
            else if (PhoneFormMode == FormMode.Edit)
            {
                CopyPhone(PhoneToModify, Phones[SelectedPhoneIndex]);
            }
            EditDialogVisible = false;
            StateHasChanged();
        }

        private void OnCancelEdit()
        {
            PhoneFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedPhone = args.Item as PhoneToWrite;
            SelectedPhoneIndex = Phones.IndexOf(SelectedPhone);
            SelectedPhones = new List<PhoneToWrite> { SelectedPhone };
        }

        private static void CopyPhone(PhoneToWrite src, PhoneToWrite dst)
        {
            dst.Id = src.Id;
            dst.Number = src.Number;
            dst.PhoneType = src.PhoneType;
            dst.IsPrimary = src.IsPrimary;
        }

    }
}
