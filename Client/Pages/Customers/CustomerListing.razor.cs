using Blazored.Modal;
using Blazored.Modal.Services;
using MenomineePlayWASM.Client.Components.Shared;
using MenomineePlayWASM.Shared.Entities.Customers;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Customers
{
    public partial class CustomerListing
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //[Inject]
        //private ICustomerRepository customerRepository { get; set; }

        [CascadingParameter]
        IModalService ModalService { get; set; }

        private long selectedId = 0;

        public List<Customer> Customers;

        private void OnEditCustomer()
        {
            if (selectedId == 0)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(ModalMessage.Message), "Please select a customer to edit.");
                ModalService.Show<ModalMessage>("Edit Item", parameters);

                //var options = new ModalOptions { HideCloseButton = true };
                //ModalService.Show<Confirm>("Hiding Close Button", options);

            }
            else
            {
                navigationManager.NavigateTo($"/customers/customer/edit/{selectedId}");
            }
        }

        private void OnDone()
        {
            navigationManager.NavigateTo("/customers/");
        }

        private void OnNewCustomer()
        {
            navigationManager.NavigateTo("/customers/customer/create/");
        }

        private void OnDeleteCustomer()
        {
            //navigationManager.NavigateTo("/customers/customer/delete/{selectedId}");
        }

        public void OnRowSelected(RowSelectEventArgs<Customer> args)
        {
            selectedId = args.Data.Id;
        }

        //protected async override Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        Customers = await customerRepository.GetCustomers();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
