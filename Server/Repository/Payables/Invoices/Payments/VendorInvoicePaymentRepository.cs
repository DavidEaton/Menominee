using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public class VendorInvoicePaymentRepository : IVendorInvoicePaymentRepository
    {
        //private readonly IHttpService httpService;
        //private string url = "api/vendorinvoicepayment";

        //public VendorInvoicePaymentRepository(IHttpService httpService)
        //{
        //    this.httpService = httpService;
        //}

        //public async Task<int> CreatePayment(VendorInvoicePayment payment)
        //{
        //    var response = await httpService.Post<VendorInvoicePayment, int>(url, payment);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<VendorInvoicePayment> GetPayment(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoicePayment>($"{url}/{id}");
        //}

        //public async Task<VendorInvoicePaymentReadDto> GetPaymentDto(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoicePaymentReadDto>($"{url}/{id}");
        //}

        //public async Task<List<VendorInvoicePayment>> GetPayments(int invoiceId)
        //{
        //    return await httpService.GetHelper<List<VendorInvoicePayment>>($"{url}/InvoiceId/{invoiceId}");
        //}

        //public async Task UpdatePayment(VendorInvoicePayment payment)
        //{
        //    var response = await httpService.Put(url, payment);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
