using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Shared
{
    public partial class ModalMessage : ComponentBase
    {
        //[CascadingParameter] BlazoredModalService BlazoredModal { get; set; }
        [CascadingParameter] IModalService ModalService { get; set; }

        [Parameter]
        public string Message { get; set; }

        //async Task CloseForm() => await ModalService.CloseAsync();
        //private void OkClick()
        //{
        //    //ModalService.Close(ModalResult.Ok(true));
        //}
    }
}
