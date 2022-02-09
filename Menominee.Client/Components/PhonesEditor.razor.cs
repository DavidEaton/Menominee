using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite Phone { get; set; }

        [Parameter]
        public IList<PhoneToWrite> Phones { get; set; }
        private bool Adding { get; set; }
        private bool Editing { get; set; }

        private bool DialogVisible => Phone != null && (Adding || Editing);
        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();
        private void Edit(PhoneToWrite item)
        {
            Editing = true;
        }

        private void Add()
        {
            Phone = new();
            Adding = true;
        }

        private void Save()
        {
            if (Adding)
                Phones.Add(Phone);

            Adding = false;
            Editing = false;
        }

        private void Cancel()
        {
            if (Phone != null && Adding)
                Phone = null;
        }

        private void CancelEditPhone()
        {
            Adding = false;
            Editing = false;
        }
    }
}
