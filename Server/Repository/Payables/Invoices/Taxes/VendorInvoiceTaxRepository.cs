using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Dtos.Payables;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes;
using MenomineePlayWASM.Shared.Entities.Payables;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public class VendorInvoiceTaxRepository : IVendorInvoiceTaxRepository
    {
        //private readonly IHttpService httpService;
        //private string url = "api/vendorinvoicetax";

        //public VendorInvoiceTaxRepository(IHttpService httpService)
        //{
        //    this.httpService = httpService;
        //}

        //public async Task<int> CreateTax(VendorInvoiceTax tax)
        //{
        //    var response = await httpService.Post<VendorInvoiceTax, int>(url, tax);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<VendorInvoiceTax> GetTax(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoiceTax>($"{url}/{id}");
        //}

        //public async Task<VendorInvoiceTaxDto> GetTaxDto(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoiceTaxDto>($"{url}/{id}");
        //}

        //public async Task<List<VendorInvoiceTax>> GetTaxes(int invoiceId)
        //{
        //    return await httpService.GetHelper<List<VendorInvoiceTax>>($"{url}/InvoiceId/{invoiceId}");
        //}

        //public async Task UpdateTax(VendorInvoiceTax tax)
        //{
        //    var response = await httpService.Put(url, tax);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
