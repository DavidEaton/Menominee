using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using System;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public class VendorInvoicePaymentMethodRepository : IVendorInvoicePaymentMethodRepository
    {
        //private readonly IHttpService httpService;
        //private string url = "api/vendorinvoicepaymentmethod";

        //public async Task<int> CreatePaymentMethod(VendorInvoicePaymentMethod method)
        //{
        //    var response = await httpService.Post<VendorInvoicePaymentMethod, int>(url, method);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<VendorInvoicePaymentMethod> GetPaymentMethod(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoicePaymentMethod>($"{url}/{id}");
        //}

        //public async Task<VendorInvoicePaymentMethodDto> GetPaymentMethodDto(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoicePaymentMethodDto>($"{url}/{id}");
        //}

        //public async Task UpdatePaymentMethod(VendorInvoicePaymentMethod method)
        //{
        //    var response = await httpService.Put(url, method);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
