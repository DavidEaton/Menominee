using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Dtos.Payables;
using MenomineePlayWASM.Shared.Entities.Payables;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public class VendorInvoiceItemRepository : IVendorInvoiceItemRepository
    {
        ////private readonly IHttpService httpService;
        //private string url = "api/vendorinvoiceitem";

        ////public VendorInvoiceItemRepository(IHttpService httpService)
        ////{
        ////    this.httpService = httpService;
        ////}

        //public Task<int> CreateItemAsync(VendorInvoiceItem item)
        //{
        //    var response = await httpService.Post<VendorInvoiceItem, int>(url, item);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<VendorInvoiceItem> GetItemAsync(int id)
        //{
        //    return await httpService.GetHelper<VendorInvoiceItem>($"{url}/{id}");
        //}

        //public async Task<IReadOnlyList<VendorInvoiceItem>> GetItemsAsync(int invoiceId)
        //{
        //    return await httpService.GetHelper<List<VendorInvoiceItem>>($"{url}/InvoiceId/{invoiceId}");
        //}

        //public async Task UpdateItemAsync(VendorInvoiceItem item)
        //{
        //    var response = await httpService.Put(url, item);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
