using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Customers
{
    public partial class Customers : ComponentBase
    {
        [Inject]
        public ICustomerDataService CustomerDataService { get; set; }

        public IReadOnlyList<CustomerToReadInList> CustomersList;
        List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();

        private CustomerToWrite CustomerToWrite { get; set; }

        private long Id { get; set; }
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
            {
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as CustomerToReadInList).Id;
            Editing = true;
            CustomersList = null;

            var readDto = await CustomerDataService.GetCustomer(Id);
            CustomerToWrite = new CustomerToWrite
            {
                //Name = new CustomerNameToWrite
                //{
                //    FirstName = readDto.FirstName,
                //    MiddleName = readDto.MiddleName,
                //    LastName = readDto.LastName
                //},
                //Gender = readDto.Gender,
                //Birthday = readDto.Birthday
            };
        }

        private void Add()
        {
            Adding = true;
            CustomersList = null;
            CustomerToWrite = new();
        }

        protected async Task AddSubmit()
        {
            await CustomerDataService.AddCustomer(CustomerToWrite);
            await Close();
        }

        protected async Task EditSubmit()
        {
            await CustomerDataService.UpdateCustomer(CustomerToWrite, Id);
            await EndEditAsync();
        }

        protected async Task Submit()
        {
            if (Adding)
                await AddSubmit();

            if (Editing)
                await EditSubmit();
        }

        protected async Task EndEditAsync()
        {
            Editing = false;
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();
        }

        protected async Task Close()
        {
            CustomerToWrite = null;
            Adding = false;
            Editing = false;
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();
        }

        private class CustomerTypeEnumModel
        {
            public CustomerType Value { get; set; }
            public string DisplayText { get; set; }
        }

    }
}
