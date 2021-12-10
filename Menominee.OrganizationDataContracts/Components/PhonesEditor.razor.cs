﻿using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite Phone { get; set; }

        [Parameter]
        public IList<PhoneToWrite> Phones { get; set; }
        private bool DialogVisible => Phone != null && (Adding || Editing);
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;

        private void Edit(PhoneToWrite item)
        {
            Phone = item;
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

            Adding = false;
            Editing = false;
        }
    }
}
